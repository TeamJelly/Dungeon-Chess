using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public float speed = 5f;
    public float mouseMoveRange = 10f;
    public float vertical, horizontal;
    public Vector2 LeftDownLimit;
    public Vector2 RightUpLimit;

    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        //if (Input.mousePosition.x <= mouseMoveRange)
        //    horizontal = -1;
        //if (Input.mousePosition.x >= Screen.width - mouseMoveRange)
        //    horizontal = 1;
        //if (Input.mousePosition.y <= mouseMoveRange)
        //    vertical = -1;
        //if (Input.mousePosition.y >= Screen.height - mouseMoveRange)
        //    vertical = 1;

        Vector3 arrow = new Vector2(horizontal, vertical);
        transform.position += arrow * Time.deltaTime * speed;

        if (transform.position.x < LeftDownLimit.x)
            transform.position = new Vector3(LeftDownLimit.x, transform.position.y, transform.position.z);
        if (transform.position.y < LeftDownLimit.y)
            transform.position = new Vector3(transform.position.x, LeftDownLimit.y, transform.position.z);
        if (transform.position.x > RightUpLimit.x)
            transform.position = new Vector3(RightUpLimit.x, transform.position.y, transform.position.z);
        if (transform.position.y > RightUpLimit.y)
            transform.position = new Vector3(transform.position.x, RightUpLimit.y, transform.position.z);


    }
}
