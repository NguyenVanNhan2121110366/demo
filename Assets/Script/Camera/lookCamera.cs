using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class lookCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5.0f;
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    [SerializeField] private float mouseX = 0;
    [SerializeField] private float mouseY = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.updateMouse();
    }
    private void updateMouse()
    {
        if(Mouse.current!=null)
        {
            mouseX=Mouse.current.delta.ReadValue().x;
            mouseY=Mouse.current.delta.ReadValue().y;
        }
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;
        // float mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity;
        // float mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity;

        player.Rotate(mouseX * Vector3.up * Time.deltaTime);
        sensitivity += mouseY * Time.deltaTime * -1.0f;
        sensitivity = Mathf.Clamp(sensitivity, -80, 80);
        transform.localRotation = Quaternion.Euler(sensitivity, 0, 0);
        //transform.Rotate(mouseY*Vector3.right*Time.deltaTime*-1);
    }
}
