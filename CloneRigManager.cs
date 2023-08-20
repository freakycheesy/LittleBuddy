using BoneLib.Nullables;
using BoneLib;
using UnityEngine;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Data;
using System;
using SLZ.VRMK;

public class CloneRigManager
{
    public string objectToDuplicateName = "[RigManager (Blank)]";
    public Vector3 offset = new Vector3(5, 0, 0);

    public void Delete()
    {
        GameObject buddyObject = GameObject.Find("Default Player Rig (0)");
        UnityEngine.Object.Destroy(buddyObject);
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
            Transform rigManager = go.transform.Find("[Rig Manager (Blank)]");
            if (rigManager != null) 
            {
                Avatar[] avatarComponents = rigManager.GetComponentsInChildren<Avatar>();
                foreach (Avatar avatar in avatarComponents)
                {
                    foreach (var mesh in avatar.hairMeshes)
                    {
                        mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                    }
                }
                Transform uiRig = rigManager.transform.Find("[UIRig]");
                if (uiRig != null)
                {
                    UnityEngine.Object.Destroy(uiRig.gameObject);
                }
                Transform spectatorCamera = rigManager.transform.Find("Spectator Camera");
                if (spectatorCamera != null)
                {
                    UnityEngine.Object.Destroy(spectatorCamera.gameObject);
                }
                Transform spawnGunUI = rigManager.transform.Find("SpawnGunUI");
                if (spawnGunUI != null)
                {
                    UnityEngine.Object.Destroy(spawnGunUI.gameObject);
                }
                Transform eventSystem = rigManager.transform.Find("EventSystem");
                if (eventSystem != null)
                {
                    UnityEngine.Object.Destroy(eventSystem.gameObject);
                }
                Transform twoDOverlay = rigManager.transform.Find("2D_Overlay");
                if (twoDOverlay != null)
                {
                    UnityEngine.Object.Destroy(twoDOverlay.gameObject);
                }
                Transform ocr = rigManager.transform.Find("[OpenControllerRig]");
                {
                    if (ocr != null)
                    {
                        Transform trackingSpace = ocr.transform.Find("TrackingSpace");
                        if (trackingSpace != null)
                        {
                            Transform headobj = trackingSpace.Find("Head");
                            if (head != null)
                            {
                                Camera cameraComponent = headobj.GetComponent<Camera>();
                                if (cameraComponent != null)
                                {
                                    cameraComponent.enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        };
        AssetSpawner.Spawn(spawnable, head.position + head.forward, default, new BoxedNullable<Vector3>(Vector3.one), false, new BoxedNullable<int>(null), null, null);
    }
}
