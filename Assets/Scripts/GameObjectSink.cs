using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GameObjectSink : MonoBehaviour
{
    public interface Sinkable
    {
	void Sink();
    }
    
    private Collider c;
    
    void Start()
    {
	c = GetComponent<Collider>();
	c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
	var s = other.gameObject.GetComponent<Sinkable>();
	if (s != null)
	{
	    s.Sink();
	}
    }
}
