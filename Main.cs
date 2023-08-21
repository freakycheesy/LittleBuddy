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
                "Rotation Degree", Color.white, 90, 5, 0, 180,
                    (float value) => cloneRigManager.rotationAmount = value
            );
            // Scale the RM
            menuCategory.CreateFunctionElement(
                "Scale Buddy", Color.white,
                    cloneRigManager.Scale
            );
            menuCategory.CreateFloatElement(
                "Scale Amount", Color.white, 1, (float)0.1, (float)0.5, 3,
                    (float value) => cloneRigManager.scaleAmount = value
            );
            // Delete the RM
            menuCategory.CreateFunctionElement(
                "Delete Buddy :(", Color.red,
                    cloneRigManager.Delete
            );
        }
    }
}
