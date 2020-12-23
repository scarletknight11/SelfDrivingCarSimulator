using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    public int neighboors = 0;
    
    void OnTriggerEnter(Collider other)
    {
	if (other.gameObject.CompareTag("Car"))
	{
	    neighboors++;
	}
    }

    void OnTriggerExit(Collider other)
    {
	if (other.gameObject.CompareTag("Car"))
	{
	    neighboors--;
	}
    }
}
