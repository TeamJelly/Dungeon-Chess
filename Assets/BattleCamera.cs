using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public float speed = 5f;
    public float mouseMoveRange = 10f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Debug.LogError($"{Input.mousePosition}, {Screen.width} {Screen.height}");

        if (Input.mousePosition.x <= mouseMoveRange)
            horizontal = -1;
        if (Input.mousePosition.x >= Screen.width - mouseMoveRange)
            horizontal = 1;
        if (Input.mousePosition.y <= mouseMoveRange)
            vertical = -1;
        if (Input.mousePosition.y >= Screen.height - mouseMoveRange)
            vertical = 1;

        Vector3 arrow = new Vector2(horizontal, vertical).normalized;

        transform.position += arrow * Time.deltaTime * speed;

    }
}
