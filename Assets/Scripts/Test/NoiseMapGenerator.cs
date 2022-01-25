using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Model.Managers;
using System.Text;
using UnityEngine.UI;

public class NoiseMapGenerator : MonoBehaviour
{
    [Range(0.001f, 1f)]
    public float scale;
    public Vector2 pos;
    [Range(0.001f, 0.8f)]
    public float intensity = 0.5f;
    //public RawImage rawImage;

    //RenderTexture renderTexture;
    int size = 12;

    bool updated = true;
    private void Start()
    {
        //renderTexture = new RenderTexture(size, size, 32);

        //renderTexture.filterMode = FilterMode.Point;

        //renderTexture.enableRandomWrite = true;
        //renderTexture.Create();

        //rawImage.texture = renderTexture;

        StartCoroutine(loop());
    }
    /*public void GenerateNoiseMap()
    {
        Texture2D texture = new Texture2D(size, size);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x ++)
            {
                Color c = Color.white;

                if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) c = Color.black;
                texture.SetPixel(x, y,c);
            }
        }

        texture.Apply();
        Graphics.Blit(texture, renderTexture);
    }*/
    public void GenerateNoiseMap2()
    {
        if (Model.Managers.FieldManager.instance == null) return;
        FieldManager.FieldData fieldData = new FieldManager.FieldData(size, size);

        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append("WL");
                else sb.Append("FR");
            }
        }
        fieldData.fieldStrData = sb.ToString();
        FieldManager.FieldData platform = new FieldManager.FieldData(size, 2, 'W', 'L');
        sb = new StringBuilder(platform.fieldStrData);
        sb[0] = 'U';
        sb[1] = 'S';
        sb[2] = 'U';
        sb[3] = 'S';
        sb[4] = 'U';
        sb[5] = 'S';
        sb[6] = 'U';
        sb[7] = 'S';
        platform.fieldStrData = sb.ToString();
        fieldData = Model.Managers.FieldManager.instance.Merge2FieldData(fieldData, platform, new Vector2Int(0, fieldData.height));
        FieldManager.instance.InitField(fieldData);
    }

    IEnumerator loop()
    {
        while(true)
        {
            if(updated)
            {
                updated = false;
                GenerateNoiseMap2();
            }
            yield return null;
        }
    }

    private void OnValidate()
    {
        updated = true;
    }
}
