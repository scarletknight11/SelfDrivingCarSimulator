using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public float radius;
    public float mass;
    public float perceptionRadius;
    public float stopCoef = 2f;

    public bool arrived
    {
	private set
	{
	    _arrived = value;
	}
	get
	{
	    return _arrived;
	}
    }

    private List<Vector3> path;
    private NavMeshAgent nma;
    private Rigidbody rb;

    private HashSet<GameObject> perceivedNeighbors = new HashSet<GameObject>();
    private HashSet<GameObject> adjacentWalls = new HashSet<GameObject>();
    private float DesiredSpeed = 1;
    private bool _arrived = true;

    void Start()
    {
	if (!AgentManager.IsAgent(gameObject))
	{
	    AgentManager.AddAgent(gameObject);
	}
        path = new List<Vector3>();
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

	radius = nma.radius;
	DesiredSpeed = nma.speed;
        rb.mass = mass;
        GetComponent<SphereCollider>().radius = perceptionRadius / 2;
    }

    private void Update()
    {
	RotateToTarget();
	if (path.Count == 0)
	{
	    arrived = true;
	    rb.velocity = Vector3.zero;
	}
        if (path.Count > 1 && Vector3.Distance(transform.position, path[0]) < 1.1f)
        {
            path.RemoveAt(0);
        } else if (path.Count == 1 && Vector3.Distance(transform.position, path[0])
		   < nma.stoppingDistance)
        {
            path.RemoveAt(0);
        }
	else if (path.Count == 1 && Vector3.Distance(transform.position, path[0])
		 < perceivedNeighbors.Count * stopCoef)
	{
	    //Debug.Log(path.Count);
	    path.RemoveAt(0);
	    SetDestination(transform.position);
	}

        #region Visualization

        if (false)
        {
            if (path.Count > 0)
            {
                Debug.DrawLine(transform.position, path[0], Color.green);
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.yellow);
            }
        }

        if (false)
        {
            foreach (var neighbor in perceivedNeighbors)
            {
                Debug.DrawLine(transform.position, neighbor.transform.position, Color.yellow);
            }
        }

        #endregion
    }

    #region Public Functions

    public void Repath()
    {
	if (nma.destination.magnitude != Mathf.Infinity)
	{
	    SetDestination(nma.destination);
	}
    }
    
    public void SetDestination(Vector3 destination)
    {
	if (Vector3.Distance(destination, nma.destination) < nma.stoppingDistance)
	{
	    return;
	}
        nma.enabled = true;
        var nmPath = new NavMeshPath();
        nma.CalculatePath(destination, nmPath);
        path = nmPath.corners.Skip(1).ToList();
        //path = new List<Vector3>() { destination };
        //nma.SetDestination(destination);
        nma.enabled = false;
	arrived = false;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    #region Incomplete Functions

    private Vector3 ComputeForce()
    {
	if (arrived)
	{
	    return Vector3.zero;
	}
	var force = CalculateGoalForce() + CalculateWallForce() + CalculateAgentForce();

        if (force != Vector3.zero)
        {
            return force.normalized * Mathf.Min(force.magnitude, Parameters.maxSpeed);
        } else
        {
            return Vector3.zero;
        }
    }
    
    private Vector3 CalculateGoalForce()
    {
        if (path.Count == 0)
        {
            return Vector3.zero;
        }

        var temp = path[0] - transform.position;
        var desiredVel = temp.normalized * DesiredSpeed;
        var actVel = rb.velocity;
        return mass * (desiredVel - actVel) / Parameters.T;
    }

    private Vector3 CalculateAgentForce()
    {
        //position to 0
         var agentForce = Vector3.zero;
        
        //for number of agents that spawn
         foreach (var n in perceivedNeighbors)
        {
            //agentmanager generates agents
            if (!AgentManager.IsAgent(n))
            {
                continue;
            }
            //agentmanager get agentobjects and render them
            var neighbor = AgentManager.agentsObjs[n];
            //calculate direction of position objects are moving
            var dir = (transform.position - neighbor.transform.position).normalized;
            //calculate collision of radius between objects and distance of position and transformation
            var overlap = (radius + neighbor.radius) - Vector3.Distance(transform.position, n.transform.position);

            //updates position of agent force distances
            agentForce += Parameters.A * Mathf.Exp(overlap / Parameters.B) * dir;
	    agentForce += -neighbor.transform.right * 30f;
	    // TODO sliding force
            //agentForce += Parameters.k * (overlap > 0f ? 1 : 0) * dir;

            //var tangent = Vector3.Cross(Vector3.up, dir);
        }
        return agentForce;
    }

    private Vector3 CalculateWallForce()
    {
	// TODO is this needed with navmesh already removing most obstacles
        var wallForce = Vector3.zero;
        foreach (var w in adjacentWalls)
        {
            var dir = (transform.position - w.transform.position).normalized;
            var overlap = (radius + 0.5f) - Vector3.Distance(transform.position, w.transform.position);

            wallForce += Parameters.A * Mathf.Exp(overlap / Parameters.B) * dir;
            wallForce += Parameters.k * Mathf.Max(overlap, 0) * dir;

            var tangent = Vector3.Cross(Vector3.up, dir);
            wallForce += Parameters.Kappa * (overlap > 0f ? overlap : 0) * Vector3.Dot(rb.velocity, tangent) * tangent;
            
        }
        return wallForce;
    }

    public void SnapToNavMesh()
    {
	// snap the agent to the nav mesh
	NavMeshHit hit;
	NavMesh.SamplePosition(
	    transform.position,
	    out hit,
	    0.2f,
	    NavMesh.AllAreas
	);
	if (Vector3.Distance(hit.position, transform.position) > 0.001f)
	{
	    NavMesh.SamplePosition(
		transform.position,
		out hit,
		2f,
		NavMesh.AllAreas
	    );

	    transform.position = hit.position;
	    /*new Vector3(
		transform.position.x,
		hit.position.y,
		transform.position.z
		);*/
	}
	
    }

    public void RotateToTarget()
    {
	if (path.Count > 0)
	{
	    var deviation = Vector3.SignedAngle(
		path[0] - transform.position,
		transform.forward,
		transform.up
	    );
	    if (Mathf.Abs(deviation) > 10f)
	    {
		var rot = Mathf.Sign(deviation) * nma.angularSpeed * Time.deltaTime;
		transform.Rotate(0f, -rot, 0f);
	    }
	    else if (Mathf.Abs(deviation) > 5f)
	    {
		transform.Rotate(0f, -deviation, 0f);
	    }
	}
    }
    
    public void ApplyForce()
    {
	SnapToNavMesh();
	// actually apply force
        var force = ComputeForce();
        //force.y = 0;

        rb.AddForce(force * 10, ForceMode.Force);
    }

    public void OnTriggerEnter(Collider other)
    {
       if (AgentManager.IsAgent(other.gameObject))
        {
            perceivedNeighbors.Add(other.gameObject);
        }
       if (other.gameObject.CompareTag("Wall"))
        {
            adjacentWalls.Add(other.gameObject);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (perceivedNeighbors.Contains(other.gameObject))
        {
            perceivedNeighbors.Remove(other.gameObject);
        }
        if (adjacentWalls.Contains(other.gameObject))
        {
            adjacentWalls.Remove(other.gameObject);
        }
    }
    
    #endregion
}
