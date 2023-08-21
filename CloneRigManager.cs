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
using SLZ.Utilities;
using UnityEngine.Rendering;

public class CloneRigManager
{
    public string objectToDuplicateName = "[RigManager (Blank)]";
    public Vector3 offset = new Vector3(5, 0, 0);
    public GameObject go;

    public void Delete()
    {
        UnityEngine.Object.Destroy(go);
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
        Action<GameObject> spawnAction = go =>
        {
            Transform rigManager = go.transform.Find("[RigManager (Blank)]");
            if (rigManager != null) 
            {
                GameObject.DestroyImmediate(rigManager.GetComponent<LineMesh>());
                GameObject.DestroyImmediate(rigManager.GetComponent<CheatTool>());
                GameObject.DestroyImmediate(rigManager.GetComponent<UtilitySpawnables>());
                GameObject.DestroyImmediate(rigManager.GetComponent<TempTextureRef>());
                GameObject.DestroyImmediate(rigManager.GetComponent<RigVolumeSettings>());
                GameObject.DestroyImmediate(rigManager.GetComponent<ForceLevels>());
                GameObject.DestroyImmediate(rigManager.GetComponent<Volume>());
                Transform physRig = rigManager.transform.Find("[PhysicsRig]");
                if (physRig != null)
                {
                    Transform physHead = physRig.transform.Find("Head");
                    if (physHead != null)
                    {
                        UnityEngine.Object.DestroyImmediate(physHead.GetComponent<WindBuffetSFX>());
                    }
                }
                UnityEngine.Object.DestroyImmediate(rigManager.GetComponent<PlayerAvatarArt>());
                Transform uiRig = rigManager.transform.Find("[UIRig]");
                if (uiRig != null)
                {
                    UnityEngine.Object.DestroyImmediate(uiRig.gameObject);
                }
                Transform spectatorCamera = rigManager.transform.Find("Spectator Camera");
                if (spectatorCamera != null)
                {
                    UnityEngine.Object.DestroyImmediate(spectatorCamera.gameObject);
                }
                Transform spawnGunUI = rigManager.transform.Find("SpawnGunUI");
                if (spawnGunUI != null)
                {
                    UnityEngine.Object.DestroyImmediate(spawnGunUI.gameObject);
                }
                Transform eventSystem = rigManager.transform.Find("EventSystem");
                if (eventSystem != null)
                {
                    UnityEngine.Object.DestroyImmediate(eventSystem.gameObject);
                }
                Transform twoDOverlay = rigManager.transform.Find("2D_Overlay");
                if (twoDOverlay != null)
                {
                    UnityEngine.Object.DestroyImmediate(twoDOverlay.gameObject);
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
                                UnityEngine.Object.DestroyImmediate(headobj.GetComponent<Camera>());
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
