using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMaterial : MonoBehaviour
{

    public float speed = 0f;
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
