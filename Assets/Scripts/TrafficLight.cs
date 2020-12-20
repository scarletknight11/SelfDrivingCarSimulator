using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public class Capture : MonoBehaviour
    {
	public TrafficLight light;
	public int grp;
	
	void Setup()
	{
	    GetComponent<Collider>().isTrigger = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
	    var w = other.GetComponent<Waiter>();
	    if (w != null)
	    {
		light.EnterGroup(grp, w);
	    }
	}

	void OnTriggerExit(Collider other)
	{
	    var w = other.GetComponent<Waiter>();
	    if (w != null)
	    {
		light.ExitGroup(grp, w);
	    }
	}
    }

    public interface Waiter
    {
	void Wait();
	void Unwait();
    }
    
    public GameObject group1Capture;
    public GameObject group2Capture;
    public float time1;
    public float time2;

    private List<Waiter>[] groups = {
	new List<Waiter>(),
	new List<Waiter>()
    };
    private float time = 0;
    private int inactiveGroup = 1; // really 2

    void Setup()
    {
	SetupGroup(1, group1Capture);
	SetupGroup(2, group2Capture);
    }

    void SetupGroup(int grp, GameObject obj)
    {
	foreach (Transform childT in obj.transform)
	{
	    var child = childT.gameObject;
	    if (child.GetComponent<Collider>() == null)
	    {
		Debug.LogError(name + " / " + child.name + " is missing a collider!");
	    }
	    var capt = child.AddComponent<Capture>();
	    capt.light = this;
	    capt.grp = grp;
	}
    }

    public void EnterGroup(int grp, Waiter w)
    {
	// TODO issue with person exiting the intersection and getting waited
	// check to see if capture and object ar facing the same way during the
	// collision?
	if (grp == inactiveGroup)
	{
	    w.Wait();
	    groups[grp].Add(w);
	}
    }

    public void ExitGroup(int grp, Waiter w)
    {
	if (grp == inactiveGroup)
	{
	    groups[grp].Remove(w);
	}
    }

    void Update()
    {
	if (time <= 0)
	{
	    inactiveGroup = (inactiveGroup + 1) % 2;
	    foreach(var waiter in groups[inactiveGroup])
	    {
		waiter.Unwait();
	    }
	    time = inactiveGroup == 0? time1 : time2;
	}
	else
	{
	    time -= Time.deltaTime;
	}
    }
}
