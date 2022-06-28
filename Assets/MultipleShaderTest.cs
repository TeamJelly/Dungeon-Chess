using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShaderTest : MonoBehaviour
{
    public List<ShaderControlTest> list;
    // Start is called before the first frame update

    int currentIndex = 0;
    ShaderControlTest currentShader;

    private void Start()
    {
        foreach(ShaderControlTest s in list)
        {
            s.DisableColor();
        }
        currentShader = list[currentIndex];
        currentShader.EnableColor();
    }

    public void Btn_ShowNext()
    {
        currentIndex = (currentIndex + 1) % list.Count;

        currentShader.DisableColor();
        currentShader = list[currentIndex];
        currentShader.EnableColor();
    }
}
