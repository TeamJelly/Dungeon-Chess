using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonData : MonoBehaviour
{
    [Serializable]
    public enum Size
    {
        XS, S, M, L, XL
    }

    [Serializable]
    public struct SizeField
    {
        public float width;
        public float height;
    }
}
