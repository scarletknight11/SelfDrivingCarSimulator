using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {

    public WayPoint previousWaypoint;
    public WayPoint nextWayPoint;

    [Range(0f, 5f)]
    public float width = 1f;

     public Vector3 GetPosition()
    {
        //calulcating atleast minimum bound to give our AI characters  enough space and distance of freedom
        Vector3 minBound = transform.position + transform.right * width / 2f;
        //calulcating atleast maximum bound to give our AI characters  enough space and distance of freedom
        Vector3 maxBound = transform.position + transform.right * width / 2f;

        //generate random points of position between min & max bounds with in environment
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
