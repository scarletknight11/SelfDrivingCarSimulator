using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PedAvoidCarBox : MonoBehaviour, Counter.SpecificCollider
{
    public GameObject waiter;
    public string tag = null;
    public string checkZoneTag = null;

    private int cars;
    private bool checking = false;
    private TrafficLight.Waiter _waiter;

    void Start()
    {
	_waiter = waiter.GetComponent<TrafficLight.Waiter>();
    }
    
    void OnTriggerEnter(Collider other)
    {
	if (other.gameObject.CompareTag(checkZoneTag))
	{
	    checking = true;
	    if (cars == 1) _waiter.Wait(false);
	}
	if (!other.gameObject.CompareTag(tag)) return;
	cars++;
	if (checking && cars > 0) _waiter.Wait(false);

    }
    
    void OnTriggerExit(Collider other)
    {
	if (other.gameObject.CompareTag(checkZoneTag))
	{
	    checking = false;
	}
	if (!other.gameObject.CompareTag(tag)) return;
	cars--;
	if (checking && cars == 0) _waiter.Unwait();
    }

    public bool IsCounterCollider(Collider other)
    {
	return false;
    }
}
