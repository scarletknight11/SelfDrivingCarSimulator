using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshObstacle))]
public class BrakeController : MonoBehaviour
{
    public float stopCoef = 3.0f;
    public float stopDist = 0.2f;
    
    private NavMeshObstacle obstacle;
    private NavMeshAgent agent;
    private Viewer viewer;
    private int neighboors = 0;

    void Start() {
        obstacle = GetComponent<NavMeshObstacle>();
	agent = GetComponent<NavMeshAgent>();
	viewer = GetComponentInChildren<Viewer>();
    }

    public void Release()
    {
	obstacle.enabled = false;
	agent.enabled = true;
    }
    
    void Update()
    {
	neighboors = viewer.neighboors;
	float dist = Vector3.Distance(transform.position, agent.destination);
	if (dist < stopCoef * neighboors || dist < stopDist)
	{
	    agent.enabled = false;
	    obstacle.enabled = true;
	}

    }
}
