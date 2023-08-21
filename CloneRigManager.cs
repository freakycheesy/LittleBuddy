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

public class CloneRigManager
{
    private GameObject defaultPlayerRigStored;
    private GameObject ocrGOStored;
    public float rotationAmount;

    public void Rotate()
    {
        ocrGOStored.transform.Rotate(Vector3.up, rotationAmount);
    }
    public void Delete()
    {
        UnityEngine.Object.Destroy(defaultPlayerRigStored);
    }
    public void Clone()
    {
        Transform head = Player.playerHead.transform;

        string barcode = "SLZ.BONELAB.Core.DefaultPlayerRig";
        SpawnableCrateReference reference = new SpawnableCrateReference(barcode);

        Spawnable spawnable = new Spawnable()
        {
            crateRef = reference
        };

        AssetSpawner.Register(spawnable);
        Action<GameObject> spawnAction = defaultPlayerRig =>
        {
            defaultPlayerRigStored = defaultPlayerRig;
            GameObject ocrGO = GameObject.Find("[OpenControllerRig]");
            ocrGOStored = ocrGO;
            Transform rigManager = defaultPlayerRig.transform.Find("[RigManager (Blank)]");
            if (rigManager != null) 
            {
                UnityEngine.Object.DestroyImmediate(rigManager.Find("EventSystem"));
                UnityEngine.Object.DestroyImmediate(rigManager.Find("[UIRig]"));
                UnityEngine.Object.DestroyImmediate(rigManager.Find("Spectator Camera"));
                UnityEngine.Object.DestroyImmediate(rigManager.Find("SpawnGunUI"));
                UnityEngine.Object.DestroyImmediate(rigManager.Find("2D_Overlay"));
                UnityEngine.Object.DestroyImmediate(rigManager.Find("Volume"));
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
        AssetSpawner.Spawn(spawnable, head.transform.position, Quaternion.identity, new BoxedNullable<Vector3>(null), false, new BoxedNullable<int>(null), spawnAction);
    }
}
