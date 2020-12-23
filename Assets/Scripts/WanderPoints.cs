using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderPoints : MonoBehaviour
{
    public static Transform points;
    public Transform autoPilotPoints;
    public static Transform autopilotPoints;
    
    void Awake()
    {
	points = transform;
	autopilotPoints = autoPilotPoints;
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

    public static Vector3 AutopilotPoint(int i)
    {
	var t = autopilotPoints.GetChild(i);
	return t.position;
    }
    
}
