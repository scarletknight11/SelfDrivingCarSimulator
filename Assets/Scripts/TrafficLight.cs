using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public class Capture : MonoBehaviour
    {
	public TrafficLight trafficLight;
	public int grp;
	
	void Start()
	{
	    GetComponent<Collider>().isTrigger = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
	    //Debug.Log("enter " + other.gameObject.name);
	    var w = other.GetComponent<Waiter>();
	    if (w != null)
	    {
		var angle = Vector3.SignedAngle(
		    transform.forward,
		    other.transform.forward,
		    Vector3.up
		);
		// exiting the intersection
		if (Mathf.Abs(angle) > 100)
		{
		    //Debug.Log("actually an exit " + angle.ToString());
		    return;
		}
		//Debug.Log(angle);
		//Debug.Log(other.gameObject.name + " wait signal");
		trafficLight.EnterGroup(grp, w);
	    }
	}

	void OnTriggerExit(Collider other)
	{
	    var w = other.GetComponent<Waiter>();
	    if (w != null)
	    {
		trafficLight.ExitGroup(grp, w);
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

    void Start()
    {
	SetupGroup(0, group1Capture);
	SetupGroup(1, group2Capture);
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
	    capt.trafficLight = this;
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
	    //Debug.Log("switch to " + inactiveGroup);
	    foreach(var waiter in groups[inactiveGroup])
	    {
		waiter.Unwait();
		
	    }
	    groups[inactiveGroup].Clear();
	    time = inactiveGroup == 0? time1 : time2;
	    inactiveGroup = (inactiveGroup + 1) % 2;
	}
	else
	{
	    time -= Time.deltaTime;
	}
    }
}
