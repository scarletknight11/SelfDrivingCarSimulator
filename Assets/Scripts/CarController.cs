using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour {

    public InputManager im;
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public float strengthCoefficient = 20000f;
    public float maxTurn = 20f;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<InputManager>();  
    }

    // Update is called once per frame
    void FixedUpdate() {

        //every time we move the throttle wheels or turn
        foreach (WheelCollider wheel in throttleWheels)
        {
            //compute torque of motor wheels vertically in order to make wheels of car turn
            wheel.motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
        }
        //every time we move the steering wheels or turn
        foreach (WheelCollider wheel in steeringWheels)
        {
            //compute torque of steering wheels vertically in order to make the whole car stir and rotate
            wheel.steerAngle = maxTurn * im.steer;
        }
    }
}
