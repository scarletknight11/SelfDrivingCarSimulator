using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour {

    public Transform CameraTarget;
    public float sSpeed = 10.0f;
    public Vector3 dist;
    public Transform looktarget;

    void FixedUpdate()
    {
        //calculate to move our full position of camera based on cameratarget position & distance vector coorrdinates
        //calculates new position frames per second
        Vector3 dPos = CameraTarget.position + dist;
        //calculates vectors coordinates between 2 points
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        //locks camera onto object vector coordinates position
        transform.LookAt(looktarget.position);
    }

}
