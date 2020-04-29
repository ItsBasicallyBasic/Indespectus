using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform cameraAxisX;
    public Transform cameraAxisY;
    public Transform camera;
    public float rotSpeed;
    public float moveSpeed;
    public float zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                cameraAxisY.Translate(0, moveSpeed * Time.deltaTime, 0);
                return;
            }
            cameraAxisX.Rotate(rotSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                cameraAxisY.Translate(0, -moveSpeed * Time.deltaTime, 0);
                return;
            }
            cameraAxisX.Rotate(-rotSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraAxisY.Rotate(0, -rotSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cameraAxisY.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }
        if(Input.mouseScrollDelta.y > 0)
        {
            camera.Translate(camera.transform.forward * zoomSpeed * Time.deltaTime);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            camera.Translate(camera.transform.forward * -zoomSpeed * Time.deltaTime);
        }
    }
}
