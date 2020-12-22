using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour, TrafficLight.Waiter, GameObjectSink.Sinkable
{

    public float waitMin = 5;
    public float waitMax = 2;
    public float walkMin = 10;
    public float walkMax = 20;

    private NavMeshAgent agent;
    private Animator animator;
    private float wait = 0;
    private float walk = 0;
    private int goal = -1;
    private bool waitingAtLight = false;

    // Start is called before the first frame update
    void Start()
    {
    agent = GetComponent<NavMeshAgent>();
	animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
	var stopped = agent.remainingDistance < agent.stoppingDistance || agent.isStopped;
	animator.SetBool("H_WALK", !stopped);
        if (wait <= 0 && walk <= 0)
	{
	    //Debug.Log("set");
	    agent.isStopped = false;
	    agent.SetDestination(WanderPoints.RandomPoint(goal));
	    walk = Random.Range(walkMin, walkMax);
	    wait = Random.Range(waitMin, waitMax);
	}
	else if (walk > 0)
	{
	    //Debug.Log("walk " + walk.ToString());
	    if (!waitingAtLight)
	    {
		walk -= Time.deltaTime;
		if (agent.remainingDistance < agent.stoppingDistance)
		{
		    agent.isStopped = true;
		    walk = 0;
		}
	    }
	}
	else if (!agent.isStopped)
	{
	    //Debug.Log(name + " walk timeout");
	    agent.isStopped = true;
	}
	else if (wait > 0)
	{
	    //Debug.Log("wait " + wait.ToString());
	    wait -= Time.deltaTime;
	}
    }

    public void Wait()
    {
	//Debug.Log("wait " + name);
	agent.isStopped = true;
	waitingAtLight = true;
    }

    public void Unwait()
    {
	if (waitingAtLight)
	{
	    //Debug.Log("unwait " + name);
	    agent.isStopped = false;
	    waitingAtLight = false;
	}
    }

    public void Sink()
    {
	Destroy(this.gameObject);
    }

}
