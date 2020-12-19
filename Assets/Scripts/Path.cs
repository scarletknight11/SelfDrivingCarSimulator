using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;

    //find stroage of waypoint lists in our nodes
    private List<Transform> nodes = new List<Transform>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        //list is empty and fresh at the beginning
        nodes = new List<Transform>();
        //picks the transform object and check if its not our own transform object
        //if it isnt it adds that path to our node object
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }

        //loop through list of nodes we colect and drawline
        for (int i = 0; i < nodes.Count; i++)
        {
            //count the node collected and draw current line
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;
            //check if our index of nodes is greater then 0 so that the next path of node is generated in proper order
            if (i > 0)
            {
                //take previous node & draw line between previous node & current node
                previousNode = nodes[i - 1].position;
                //previous node is our last node in the list
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previousNode = nodes[nodes.Count - 1].position;
            }
            //draw path finding line of previous and current node & connect them
            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}
