using BoneLib.Nullables;
using BoneLib;
using UnityEngine;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Data;
using System;
using SLZ.VRMK;
using SLZ.Bonelab;
using SLZ.Marrow;
using UnityEngine.Rendering.Universal;
using SLZ.SFX;
using SLZ.Rig;
using SLZ.UI;
using UnityEngine.Rendering;
using MelonLoader;


namespace LittleBuddy
{
    public class CloneRigManager
    {
        public float rotationAmount;

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
                marker.transform.Rotate(Vector3.up, rotationAmount);
            }
        }
        public void Delete()
        {
            // Get every object with the DefaultPlayerRigMarker component and delete them all, in case the player spawned many clones
            DefaultPlayerRigMarker[] rmObj = UnityEngine.Object.FindObjectsOfType <DefaultPlayerRigMarker>();
            foreach (DefaultPlayerRigMarker marker in rmObj)
            {
                UnityEngine.Object.Destroy(marker.gameObject);
            }
        }
        public void Clone()
        {
            // Makes sure the clone spawns a bit away from the player
            Vector3 position = Player.physicsRig.m_chest.gameObject.transform.position;
            position.z += 3f;

            string barcode = "SLZ.BONELAB.Core.DefaultPlayerRig";
            SpawnableCrateReference reference = new SpawnableCrateReference(barcode);

            Spawnable spawnable = new Spawnable()
            {
                crateRef = reference
            };

            AssetSpawner.Register(spawnable);
            MelonLogger.Msg("Fixing rig manager");
            Action<GameObject> spawnAction = defaultPlayerRig =>
            {
                // Cleans up the rigmanager to prevent UI breaking
                defaultPlayerRig.AddComponent<DefaultPlayerRigMarker>();
                Transform playerRigTransform = defaultPlayerRig.transform;
                Transform eventSystem = defaultPlayerRig.transform.Find("EventSystem");
                UnityEngine.Object.Destroy(eventSystem.gameObject);
                Transform rigManager = defaultPlayerRig.transform.Find("[RigManager (Blank)]");
                if (rigManager != null)
                {
                    // Make the rigmanager unable to take damage
                    Player_Health playerHealth = rigManager.GetComponent<Player_Health>();
                    playerHealth.damageFromAttack = false;
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
                    // Useful for controlling parts of the rigmanager later
                    rigManager.gameObject.AddComponent<RigManagerMarker>();
                    // We don't actually want to destroy the UI rig, it may break something, so I disable it.
                    Transform uiRig = rigManager.Find("[UIRig]");
                    //UnityEngine.Object.DestroyImmediate(uiRig.gameObject);
                    uiRig.gameObject.SetActive(false);
                    // A clone doesn't need a spectator camera.
                    Transform specCam = rigManager.Find("Spectator Camera");
                    UnityEngine.Object.DestroyImmediate(specCam.gameObject);
                    // Deleting the spawngunUI entirely breaks your spawngunUI, so we just disable it instead.
                    Transform spawnUI = rigManager.Find("SpawnGunUI");
                    spawnUI.gameObject.SetActive(false);
                    // A clone doesn't need a canvas for the screen.
                    Transform overlay = rigManager.Find("2D_Overlay");
                    UnityEngine.Object.DestroyImmediate(overlay.gameObject);
                    // Removes any extra bloom that the rigmanager usually adds
                    UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<Volume>());
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
                            // Useful for disabling all input later
                            ocr.gameObject.AddComponent<OpenControllerRigMarker>();
                            // Disables unneeded inputs
                            OpenControllerRig ocrc = ocr.GetComponent<OpenControllerRig>();
                            ocrc.primaryEnabled = true;
                            ocrc.jumpEnabled = true;
                            ocrc.quickmenuEnabled = false;
                            ocrc.slowMoEnabled = false;
                            ocrc.autoLiftLegs = true;
                            ocrc.doubleJump = false;

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
            AssetSpawner.Spawn(spawnable, position, Quaternion.identity, new BoxedNullable<Vector3>(null), false, new BoxedNullable<int>(null), spawnAction);
        }
    }
}