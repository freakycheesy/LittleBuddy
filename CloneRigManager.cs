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
        public void Rotate()
        {
            RigManagerMarker[] rotObj = UnityEngine.Object.FindObjectsOfType<RigManagerMarker>();
            foreach (RigManagerMarker marker in rotObj)
            {
                marker.transform.Rotate(Vector3.up, rotationAmount);
            }
        }
        public void Delete()
        {
            RigManagerMarker[] rmObj = UnityEngine.Object.FindObjectsOfType<RigManagerMarker>();
            foreach (RigManagerMarker marker in rmObj)
            {
                UnityEngine.Object.Destroy(marker.gameObject);
            }
        }
        public void Clone()
        {
            MelonLogger.Msg("Storing player position");
            Vector3 position = Player.physicsRig.m_chest.gameObject.transform.position;
            position.z += 5f;

            MelonLogger.Msg("Spawning rig manager");
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
                defaultPlayerRig.AddComponent<RigManagerMarker>();
                Transform playerRigTransform = defaultPlayerRig.transform;
                Transform eventSystem = defaultPlayerRig.transform.Find("EventSystem");
                GameObject eventSystemGO = eventSystem.gameObject;
                UnityEngine.Object.Destroy(eventSystemGO);
                Transform rigManager = defaultPlayerRig.transform.Find("[RigManager (Blank)]");
                if (rigManager != null)
                {
                    Transform uiRig = rigManager.Find("[UIRig]");
                    GameObject uiRigGO = uiRig.gameObject;
                    UnityEngine.Object.DestroyImmediate(uiRigGO);
                    Transform specCam = rigManager.Find("Spectator Camera");
                    GameObject specCamGO = specCam.gameObject;
                    UnityEngine.Object.DestroyImmediate(specCamGO);
                    //MelonLogger.Msg("SpawnGunUI");
                    //Transform spawnUI = rigManager.Find("SpawnGunUI");
                    // GameObject spawnUIGO = spawnUI.gameObject;
                    //UnityEngine.Object.DestroyImmediate(spawnUIGO);
                    Transform overlay = rigManager.Find("2D_Overlay");
                    GameObject overlayGO = overlay.gameObject;
                    UnityEngine.Object.DestroyImmediate(overlayGO);
                    UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<Volume>());
                    UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<PlayerAvatarArt>());
                    Transform physRig = rigManager.transform.Find("[PhysicsRig]");
                    if (physRig != null)
                    {
                        Transform physHead = physRig.transform.Find("Head");
                        if (physHead != null)
                        {
                            UnityEngine.Object.DestroyImmediate(physHead.GetComponent<WindBuffetSFX>());
                        }
                    }
                    Transform ocr = rigManager.transform.Find("[OpenControllerRig]");
                    {
                        if (ocr != null)
                        {
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
                                Transform rHand = trackingSpace.Find("Hand (right)");
                                if (rHand != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(rHand.GetComponent<UIControllerInput>());
                                    Haptor rHandHaptor = rHand.GetComponent<Haptor>();
                                    rHandHaptor.hapticsAllowed = false;
                                }
                                Transform lHand = trackingSpace.Find("Hand (left)");
                                if (lHand != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(lHand.GetComponent<UIControllerInput>());
                                    Haptor lHandHaptor = lHand.GetComponent<Haptor>();
                                    lHandHaptor.hapticsAllowed = false;
                                }
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