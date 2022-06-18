using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PixelCameraSize : MonoBehaviour
{
    public int divideNumber = 4;

    public PixelPerfectCamera pcamera;

    // Start is called before the first frame update
    void Start()
    {
        pcamera.refResolutionX = Screen.width / divideNumber;
        pcamera.refResolutionY = Screen.height / divideNumber;
    }
}
