using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

    private Transform startPosition; 
    private Transform endPosition;
    
    public Node startNode { get; set; }
    
    public Node goalNode { get; set; }

    private ArrayList pathArray;
    private GameObject startCube;
    private GameObject endCube;
    private float elapsedTime = 0.0f; 
    public float intervalTime = 1.0f; 
    private GridManager gridManager;

    // Start is called before the first frame update
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>(); 
        startCube = GameObject.FindGameObjectWithTag("Start"); 
        endCube = GameObject.FindGameObjectWithTag("End");
    }

    // Update is called once per frame
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime) { 
            elapsedTime = 0.0f; 
            FindPath(); 
        }
    }

    //It takes the positions of our start and end game objects. 
    //Then, it creates new Node objects using the helper methods GridManager and GetGridIndex to calculate their respective row and column index positions inside the grid. 
    private void FindPath()
    {
        startPosition = startCube.transform; 
        endPosition = endCube.transform; 
        startNode = new 
            Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(startPosition.position)));
        goalNode = new 
            Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(endPosition.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
    }

    private void OnDrawGizmos()
    {
        if (pathArray == null)
        {
            return;
        }
        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }
}
