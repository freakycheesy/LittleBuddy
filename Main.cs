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
                "Rotation Speed", Color.white, 90, 5, 0, 180,
                    (float value) => cloneRigManager.rotationAmount = value
            );
            // Delete the RM
            menuCategory.CreateFunctionElement(
                "Delete Buddy :(", Color.red,
                    cloneRigManager.Delete
            );
        }
    }
}
