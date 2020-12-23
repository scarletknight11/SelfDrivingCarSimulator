using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSpeed = 3f;
    public float moveSpeed = 3.0f;

    void Update()
    {
	// move camera
	var rotation = transform.localEulerAngles;
	rotation.y += Input.GetAxis("Mouse X") * mouseSpeed;
	rotation.x += -Input.GetAxis("Mouse Y") * mouseSpeed;
	transform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0);
    }

    void FixedUpdate()
    {
        Vector3 horiz = Input.GetAxis("Horizontal") * transform.right;
	Vector3 vert = Input.GetAxis("Vertical") * transform.forward;
	Vector3 jump = Input.GetAxis("Jump") * transform.up;
	transform.position += (horiz + jump + vert) * moveSpeed * Time.deltaTime;
	
    }
}
