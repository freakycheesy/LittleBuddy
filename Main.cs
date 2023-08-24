using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu.Elements;
using BoneLib.BoneMenu;

namespace LittleBuddy
{
    internal partial class Main : MelonMod
    {
        public static CloneRigManager cloneRigManager;
        public override void OnInitializeMelon()
        {
            cloneRigManager = new CloneRigManager();
            SetupBonemenu();
        }
        public static void SetupBonemenu()
        {
            MenuCategory menuCategory = MenuManager.CreateCategory("Little Buddy", Color.yellow);
            // Clone the RM
            menuCategory.CreateFunctionElement(
                "Spawn Buddy :D",
                Color.green,
                    cloneRigManager.Clone
            );
            // Rotate the RM
            menuCategory.CreateFunctionElement(
                "Rotate Buddy", Color.white,
                    cloneRigManager.Rotate
            );
            menuCategory.CreateFloatElement(
                "Rotation Degree", Color.white, 90, 5, -180, 180,
                    (float value) => cloneRigManager.rotationAmount = value
            );
            // Freeze the RM
            menuCategory.CreateFunctionElement(
                "Freeze Buddy", Color.cyan,
                    cloneRigManager.Freeze
            );
            // Unfreeze the RM
            menuCategory.CreateFunctionElement(
                "Unfreeze Buddy", Color.yellow,
                    cloneRigManager.Unfreeze
            );
            // Delete the RM, which really is just reloading the scene
            menuCategory.CreateFunctionElement(
                "Reload Scene", Color.red,
                    cloneRigManager.Delete
            );
        }
    }
}
