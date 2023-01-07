using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectView : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 20.0f;

    private void Update()
    {
        //Only rotates the object

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        Vector3 rotatObjx = Camera.main.transform.TransformDirection(new Vector3(0f, 1f, 0f)) ;
        //Vector3 rotatObjy = Camera.main.transform.TransformDirection(new Vector3( 1f, 0f, 0f));

        //transform.RotateAround(transform.position, rotatObjy, mouseY);
        transform.RotateAround(transform.position, rotatObjx, -mouseX);
    }
}
