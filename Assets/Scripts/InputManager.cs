using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public float throttle;
    public float steer;

    // Update is called once per frame
    void Update()
    {
        throttle = Input.GetAxis("vertical");
        steer = Input.GetAxis("Horizontal");
    }
}
