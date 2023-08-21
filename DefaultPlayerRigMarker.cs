using MelonLoader;
using UnityEngine;
using System;

namespace LittleBuddy
{
    [RegisterTypeInIl2Cpp]
    public class DefaultPlayerRigMarker : MonoBehaviour
    {
        #if !UNITY_EDITOR
            public DefaultPlayerRigMarker(IntPtr ptr) : base(ptr) { }
        #endif
    }
}
