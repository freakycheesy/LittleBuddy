using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu.Elements;
using BoneLib.BoneMenu;

namespace LittleBuddy
{
    public static class BuildInfo
    {
        public const string Name = "LittleBuddy"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "A little friend :)"; // Description of the Mod. (Set as null if none)
        public const string Author = "CarrionAndOn"; // Author of the Mod.  (Set as null if none)
        public const string Company = "null"; // Company that made the Mod.  (Set as null if none)
        public const string Version = "0.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "null"; // Download Link for the Mod.  (Set as null if none)

    }
    public class LittleBuddy : MelonMod
    {
        public static CloneRigManager rm;
        public override void OnInitializeMelon()
        {
            rm = new CloneRigManager();
            SetupBonemenu();
        }
        public static void SetupBonemenu()
        {
            MenuCategory menuCategory = MenuManager.CreateCategory("Little Buddy", Color.yellow);
            // Create toggle all bonemenu button
            menuCategory.CreateFunctionElement(
                "Clone Rig Manager",
                Color.white,
                    rm.Clone
            );
            // Create toggle all bonemenu button
            menuCategory.CreateFunctionElement(
                "Delete Cloned Rig Manager",
                Color.red,
                    rm.Delete
            );
        }
    }
}
