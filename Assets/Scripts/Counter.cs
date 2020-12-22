using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Counter : MonoBehaviour
{
    public bool reverse = false;
    public int count = 0;
    private Collider c;
    
    void Start()
    {
	c = GetComponent<Collider>();
	c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {	
	var angle = Vector3.SignedAngle(
	    transform.forward,
	    other.transform.forward,
	    Vector3.up
	);

	if (!reverse && Mathf.Abs(angle) > 100) return;
	else if (reverse && Mathf.Abs(angle) < 80) return;
	count++;
    }
}
