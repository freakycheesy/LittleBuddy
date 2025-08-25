﻿using BoneLib;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.Marrow.SceneStreaming;
using Il2CppSLZ.Marrow.Data;
using Il2CppSLZ.Marrow.Pool;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Audio;
using Il2CppSLZ.Bonelab;
using Il2Cpp;

namespace LittleBuddy
{
    public class CloneRigManager
    {
        public float RotationAmount;

        public void Freeze()
        {
            // Get the default player rig
            DefaultPlayerRigMarker[] defaultPlayerRig = UnityEngine.Object.FindObjectsOfType<DefaultPlayerRigMarker>();
            foreach (DefaultPlayerRigMarker playerRig in defaultPlayerRig)
            {
                // Get all rigidbodies
                Rigidbody[] rbs = playerRig.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rbs)
                {
                    // Essentially freeze the rigidbody
                    rb.isKinematic = true;
                }
            }
        }

        public void Unfreeze()
        {
            // Get the default player rig
            DefaultPlayerRigMarker[] defaultPlayerRig = UnityEngine.Object.FindObjectsOfType<DefaultPlayerRigMarker>();
            foreach (DefaultPlayerRigMarker playerRig in defaultPlayerRig)
            {
                // Get all rigidbodies
                Rigidbody[] rbs = playerRig.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rbs)
                {
                    // Unfreeze the rigidbody
                    rb.isKinematic = false;
                }
            }
        }
        public void Rotate()
        {
            // Get every object with the DefaultPlayerRigMarker component and rotate them all
            DefaultPlayerRigMarker[] rotObj = UnityEngine.Object.FindObjectsOfType<DefaultPlayerRigMarker>();
            foreach (DefaultPlayerRigMarker marker in rotObj)
            {
                marker.transform.Rotate(Vector3.up, RotationAmount);
            }
        }
        public void Delete()
        {
            SceneStreamer.Reload();
        }
        public void Clone()
        {
            // Makes sure the clone spawns a bit away from the player
            Vector3 position = Player.PhysicsRig.m_chest.gameObject.transform.position;
            position.z += 3f;

            string barcode = "SLZ.BONELAB.Core.DefaultPlayerRig";
            SpawnableCrateReference reference = new SpawnableCrateReference(barcode);

            Spawnable spawnable = new Spawnable()
            {
                crateRef = reference
            };

            AssetSpawner.Register(spawnable);
            // Cleans up the rigmanager to prevent UI breaking
            Action<GameObject> spawnAction = defaultPlayerRig =>
            {
                defaultPlayerRig.AddComponent<DefaultPlayerRigMarker>();
                // Prevents my black hole from deleting it
                Transform playerRigTransform = defaultPlayerRig.transform;
                Transform eventSystem = defaultPlayerRig.transform.Find("EventSystem");
                UnityEngine.Object.Destroy(eventSystem.gameObject);
                Transform rigManager = defaultPlayerRig.transform.Find("[RigManager (Blank)]");
                if (rigManager != null)
                {
                    // Make the rigmanager unable to take damage
                    Player_Health playerHealth = rigManager.GetComponent<Player_Health>();
                    playerHealth.damageFromImpact = false;
                    playerHealth.healthMode = Health.HealthMode.Invincible;
                    // Remove all damage recievers so you don't take damage when the clone does
                    PlayerDamageReceiver[] damageReceivers = UnityEngine.Object.FindObjectsOfType<PlayerDamageReceiver>();
                    foreach (PlayerDamageReceiver damageReceiver in damageReceivers)
                    {
                        UnityEngine.Object.Destroy(damageReceiver);
                    }
                    // Remove all trigger proxies so that we don't trigger scene zones
                    PlayerTriggerProxy[] triggerProxies = UnityEngine.Object.FindObjectsOfType<PlayerTriggerProxy>();
                    foreach (PlayerTriggerProxy triggerProxy in triggerProxies)
                    {
                        UnityEngine.Object.Destroy(triggerProxy);
                    }
                    // We don't actually want to destroy the UI rig, it may break something, so I disable it.
                    Transform uiRig = rigManager.Find("[UIRig]");
                    //UnityEngine.Object.DestroyImmediate(uiRig.gameObject);
                    uiRig.gameObject.SetActive(false);
                    // Fusion does this to their UI rig, so let's do that too
                    UIRig uiRigC = uiRig.GetComponent<UIRig>();
                    uiRigC.Start();
                    uiRigC.popUpMenu.radialPageView.Start();
                    try
                    {
                        uiRigC.popUpMenu.Start();
                    }
                    catch { }
                    // Destroy unneeded datamanager
                    UnityEngine.Object.DestroyImmediate(uiRig.Find("DATAMANAGER").gameObject);
                    // A clone doesn't need a spectator camera.
                    Transform specCam = rigManager.Find("Spectator Camera");
                    UnityEngine.Object.DestroyImmediate(specCam.gameObject);
                    // Deleting the spawngunUI entirely breaks your spawngunUI, so we just disable it instead.
                    Transform spawnUI = rigManager.Find("SpawnGunUI");
                    spawnUI.gameObject.SetActive(false);
                    // A clone doesn't need a canvas for the screen.
                    Transform overlay = rigManager.Find("2D_Overlay");
                    UnityEngine.Object.DestroyImmediate(overlay.gameObject);
                    // Remove unneeded components on the rigmanager
                    UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<Volume>());
                    // Avatar stuff i think
                    RigManager rigManagerC = rigManager.GetComponent<RigManager>();
                    // Fixes the hair mesh not appearing
                    UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<PlayerAvatarArt>());
                    Transform physRig = rigManager.transform.Find("[PhysicsRig]");
                    if (physRig != null)
                    {
                        Transform physHead = physRig.transform.Find("Head");
                        if (physHead != null)
                        {
                            // We don't want you to hear the clone's wind noises.
                            UnityEngine.Object.DestroyImmediate(physHead.GetComponent<WindBuffetSFX>());
                            // This does something, I'm not sure what, but we don't need it.
                            UnityEngine.Object.DestroyImmediate(physHead.GetComponent<MusicAmbience2dSFX>());
                        }
                    }
                    Transform ocr = rigManager.transform.Find("[OpenControllerRig]");
                    {
                        if (ocr != null)
                        {
                            // Disables unneeded inputs
                            OpenControllerRig ocrc = ocr.GetComponent<OpenControllerRig>();
                            //ocrc.primaryEnabled = true;
                            //ocrc.jumpEnabled = true;
                            //ocrc.quickmenuEnabled = false;
                            //ocrc.slowMoEnabled = false;
                            //ocrc.autoLiftLegs = true;
                            //ocrc.doubleJump = false;

                            Transform trackingSpace = ocr.transform.Find("TrackingSpace");
                            if (trackingSpace != null)
                            {
                                // Unneeded scripts, may cause issues with UI
                                Transform rHand = trackingSpace.Find("Hand (right)");
                                if (rHand != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(rHand.GetComponent<UIControllerInput>());
                                    Haptor rHandHaptor = rHand.GetComponent<Haptor>();
                                    rHandHaptor.hapticsAllowed = false;
                                }
                                // Unneeded scripts, may cause issues with UI
                                Transform lHand = trackingSpace.Find("Hand (left)");
                                if (lHand != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(lHand.GetComponent<UIControllerInput>());
                                    Haptor lHandHaptor = lHand.GetComponent<Haptor>();
                                    lHandHaptor.hapticsAllowed = false;
                                }
                                // Remove the camera from the head
                                Transform headobj = trackingSpace.Find("Head");
                                if (headobj != null)
                                {
                                    headobj.tag = "Untagged";
                                    Camera cameraComponent = headobj.GetComponent<Camera>();
                                    if (cameraComponent != null)
                                    {
                                        cameraComponent.enabled = false;
                                    }
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<AudioListener>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<DebugDraw>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<XRLODBias>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<VolumetricPlatformSwitch>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<StreamingController>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<VolumetricRendering>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<UniversalAdditionalCameraData>());
                                    UnityEngine.Object.DestroyImmediate(headobj.GetComponent<CameraSettings>());
                                }
                            }
                        }
                    }
                }
            };
            AssetSpawner.Spawn(spawnable, position, Quaternion.identity, null, null, false, null, spawnAction);
        }
    }
}