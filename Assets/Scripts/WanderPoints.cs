using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderPoints : MonoBehaviour
{
    public static Transform points;
    
    void Start()
    {
	points = transform;
    }
    
    public static Vector3 RandomPoint(int goal)
    {
	var newGoal = Random.Range(0, points.childCount);
	if (goal == newGoal) return RandomPoint(goal);
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
