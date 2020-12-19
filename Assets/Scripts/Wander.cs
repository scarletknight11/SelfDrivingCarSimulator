using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour {

    public Transform points;
    public float waitMin = 5;
    public float waitMax = 2;
    public float walkMin = 10;
    public float walkMax = 20;

    private NavMeshAgent agent;
    private Animator animator;
    private float wait = 0;
    private float walk = 0;
    private int goal = -1;

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
	    agent.SetDestination(RandomPoint());
	    walk = Random.Range(walkMin, walkMax);
	    wait = Random.Range(waitMin, waitMax);
	}
	else if (walk > 0)
	{
	    //Debug.Log("walk " + walk.ToString());
	    walk -= Time.deltaTime;
	    if (agent.remainingDistance < agent.stoppingDistance)
	    {
		agent.isStopped = true;
		walk = 0;
	    }
	}
	else if (!agent.isStopped)
	{
	    //Debug.Log("walk timeout");
	    agent.isStopped = true;
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
}
