using UnityEngine;
public class CloneRigManager
{
    public string objectToDuplicateName = "[RigManager (Blank)]";
    public Vector3 offset = new Vector3(5, 0, 0);

    public void Delete()
    {
        GameObject buddyObject = GameObject.Find("LittleBuddy");
        Object.Destroy(buddyObject);
    }
    public void Clone()
    {
        // Find the rig manager
        GameObject originalObject = GameObject.Find(objectToDuplicateName);

        if (originalObject != null)
        {
            // Duplicate the rig manager
            GameObject duplicatedObject = Object.Instantiate(originalObject);

            // Offset the rigmanager clone by 5 on X axis
            duplicatedObject.transform.position += offset;

            // Remove UI related objects from the rig manager
            Transform spectatorCamera = duplicatedObject.transform.Find("Spectator Camera");
            if (spectatorCamera != null)
            {
                Object.Destroy(spectatorCamera.gameObject);
            }

            Transform uiRig = duplicatedObject.transform.Find("[UIRig]");
            if (uiRig != null)
            {
                Object.Destroy(uiRig.gameObject);
            }

            Transform spawnGunUI = duplicatedObject.transform.Find("SpawnGunUI");
            if (spawnGunUI != null)
            {
                Object.Destroy(spawnGunUI.gameObject);
            }

            Transform twoDOverlay = duplicatedObject.transform.Find("2D_Overlay");
            if (twoDOverlay != null)
            {
                Object.Destroy(twoDOverlay.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Object to duplicate not found: " + objectToDuplicateName);
        }
    }
}
