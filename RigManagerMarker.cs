using MelonLoader;
using UnityEngine;
using System;

namespace LittleBuddy
{
    [RegisterTypeInIl2Cpp]
    public class RigManagerMarker : MonoBehaviour
    {
        #if !UNITY_EDITOR
            public RigManagerMarker(IntPtr ptr) : base(ptr) { }
        #endif
    }
}
