using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu.Elements;
using BoneLib.BoneMenu;

namespace LittleBuddy
{
    internal partial class Main : MelonMod
    {
        private static CloneRigManager _cloneRigManager;
        public override void OnInitializeMelon()
        {
            _cloneRigManager = new CloneRigManager();
            SetupBonemenu();
        }
        public static void SetupBonemenu()
        {
            MenuCategory menuCategory = MenuManager.CreateCategory("Little Buddy", Color.yellow);
            // Clone the RM
            menuCategory.CreateFunctionElement(
                "Spawn Buddy :D",
                Color.green,
                    _cloneRigManager.Clone
            );
            // Rotate the RM
            menuCategory.CreateFunctionElement(
                "Rotate Buddy", Color.white,
                    _cloneRigManager.Rotate
            );
            menuCategory.CreateFloatElement(
                "Rotation Degree", Color.white, 90, 5, -180, 180,
                    (float value) => _cloneRigManager.RotationAmount = value
            );
            // Freeze the RM
            menuCategory.CreateFunctionElement(
                "Freeze Buddy", Color.cyan,
                    _cloneRigManager.Freeze
            );
            // Unfreeze the RM
            menuCategory.CreateFunctionElement(
                "Unfreeze Buddy", Color.yellow,
                    _cloneRigManager.Unfreeze
            );
            // Delete the RM, which really is just reloading the scene
            menuCategory.CreateFunctionElement(
                "Reload Scene", Color.red,
                    _cloneRigManager.Delete
            );
        }
    }
}
