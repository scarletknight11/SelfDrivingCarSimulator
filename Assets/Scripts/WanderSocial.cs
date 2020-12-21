using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderSocial : MonoBehaviour, TrafficLight.Waiter
{
    public Transform points;
    public float waitMin = 5;
    public float waitMax = 2;
    public float walkMin = 10;
    public float walkMax = 20;

    private Agent agent;
    private NavMeshAgent nma;
    private Animator animator;
    private Rigidbody rb;
    private float wait = 0;
    private float walk = 0;
    private int goal = -1;

    private Vector3 destination = Vector3.positiveInfinity;
    private bool waitingAtLight = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Agent>();
	nma = GetComponent<NavMeshAgent>();
	animator = GetComponent<Animator>();
	rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
	animator.SetBool("H_WALK", !agent.arrived);
        if (wait <= 0 && walk <= 0)
	{
	    //Debug.Log("set");
	    destination = RandomPoint();
	    agent.SetDestination(destination);
	    
	    walk = Random.Range(walkMin, walkMax);
	    wait = Random.Range(waitMin, waitMax);
	}
	else if (walk > 0)
	{
	    //Debug.Log("walk " + walk.ToString());
	    if (!waitingAtLight)
	    {
		walk -= Time.deltaTime;
		if (agent.arrived)
		{
		    walk = 0;
		}
	    }
	}
	else if (!agent.arrived)
	{
	    //Debug.Log(name + " walk timeout");
	    agent.SetDestination(transform.position);
	}
	else if (wait > 0)
	{
	    //Debug.Log("wait " + wait.ToString());
	    wait -= Time.deltaTime;
	}
    }

    Vector3 RandomPoint()
    {
	var newGoal = Random.Range(0, points.childCount);
	if (goal == newGoal) return RandomPoint();
	goal = newGoal;
	var point = points.GetChild(newGoal).position;
	// find closest point on nav mesh within 1u
	NavMeshHit hit;
	var found = NavMesh.SamplePosition(
	    point,
	    out hit,
	    1f,
	    NavMesh.AllAreas
	);
	return hit.position;
    }

    public void Wait()
    {
	//Debug.Log("wait " + name);
	agent.SetDestination(transform.position);
	
	waitingAtLight = true;
    }

    public void Unwait()
    {
	if (waitingAtLight)
	{
	    //Debug.Log("unwait " + name + " " + destination.ToString());
	    waitingAtLight = false;
	    agent.SetDestination(destination);
	}
    }
}
