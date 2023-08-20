using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu.Elements;
using BoneLib.BoneMenu;

namespace LittleBuddy
{
    internal partial class Main : MelonMod
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
            // Clone the RM
            menuCategory.CreateFunctionElement(
                "Spawn Buddy",
                Color.white,
                    rm.Clone
            );
            // Delete the RM
            menuCategory.CreateFunctionElement(
                "Delete Buddy",
                Color.red,
                    rm.Delete
            );
        }
    }
}
