using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarEngine : MonoBehaviour {

    public Transform path;
    public float maxSteerAngle = 45f;
    public WheelCollider wheekFL;
    public WheelCollider wheekFR;
    public float maxMotorTorque = 50f;
    public float currentSpeed;
    public float MaxSpeed = 100f;

    private List<Transform> nodes;
    private int currentNode = 0;

    // Start is called before the first frame update
    private void Start() {
        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        //list is empty and fresh at the beginning
        nodes = new List<Transform>();
        //picks the transform object and check if its not our own transform object
        //if it isnt it adds that path to our node object
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

   

    private void ApplySteer()
    {
        //steer to position of node found in next path point
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheekFL.steerAngle = newSteer;
        wheekFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheekFL.radius * wheekFL.rpm * 60 / 1000;

        //if current current of motortorque is > then maxspeed then pply torque
        if (currentSpeed < MaxSpeed)
        {
            wheekFL.motorTorque = maxMotorTorque;
            wheekFR.motorTorque = maxMotorTorque;
        //else keep going
        } else
        {
            wheekFL.motorTorque = 0;
            wheekFR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        //if transformation distance of car is less then position of path finding node of value 0.05
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
        {
            //if currentnode value is same as pevious node value
            if(currentNode == nodes.Count - 1) {
                //currentnode is now empty
                currentNode = 0;
            } else
            {
                //increase number of current node
                currentNode++;
            }

        }
    }
}
