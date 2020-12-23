using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerController : MonoBehaviour
{
    public CamFollowPlayer controller1;
    public CameraController controller2;

    void Update()
    {
	if (Input.GetKeyDown("f"))
	{
	    var state = controller1.enabled;
	    controller1.enabled = !state;
	    controller2.enabled = state;
	}
    }
}
