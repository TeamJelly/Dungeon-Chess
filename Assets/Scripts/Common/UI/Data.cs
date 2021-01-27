using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Common.UI
{
    public class Data : MonoBehaviour
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
}
