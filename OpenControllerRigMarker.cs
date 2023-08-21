using MelonLoader;
using UnityEngine;
using System;

namespace LittleBuddy
{
    [RegisterTypeInIl2Cpp]
    public class OpenControllerRigMarker : MonoBehaviour
    {
        #if !UNITY_EDITOR
            public OpenControllerRigMarker(IntPtr ptr) : base(ptr) { }
        #endif
    }
}
