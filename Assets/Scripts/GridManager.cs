using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField]
    private int numberOfRows = 20;
    [SerializeField]
    public int numberOfColumns = 20;
    [SerializeField]
    public float gridCellSize = 2;
    [SerializeField]
    public bool showGrid = true;
    [SerializeField]
    public bool showObstacleBlocks = true;

    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    private Node[,] nodes { get; set; }

    void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private void InitializeNodes()
    {
        nodes = new Node[numberOfColumns, numberOfRows];

        int index = 0;
        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                Vector3 cellPosition = GetGridCellCenter(index);
                Node node = new Node(cellPosition);
                nodes[i, j] = node;
                index++;
            }
        }
    }



    private void CalculateObstacles()
    {
        if (obstacleList != null & obstacleList.Length > 0)
        {
            foreach (GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int column = GetColumnOfIndex(indexCell);
                int row = GetRowOfIndex(indexCell);
                nodes[row, column].MarkAsObstacle();
            }
        }
    }

    internal int GetGridIndex(object position)
    {
        throw new NotImplementedException();
    }

    internal Vector3 GetGridCellCenter(int v)
    {
        throw new NotImplementedException();
    }

    public void GetNeighbors(Node node, ArrayList neighbors)
    {
        Vector3 neighborPosition = node.position;
        int neighborIndex = GetGridIndex(neighborPosition);

        int row = GetRowOfIndex(neighborIndex); 
        int column = GetColumnOfIndex(neighborIndex);

        //Bottom
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        //Top
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        //Right
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        //Left
        leftNodeRow = row; 
        leftNodeColumn = column - 1; 
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);
    }

    // Check the neighbor. If it's not an obstacle, assign the neighbor.
    private void AssignNeighbor(int row, int column, ArrayList neighbors)
    {
         if(row != -1 && column != -1 && row < numberOfRows && column < numberOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }


    //Given an index for a cell, it returns the center position (in world space) of that cell. 
    //private Vector3 GetGridCellCenter(int index)
    //{
    //    throw new NotImplementedException();
    //}
    //returns the row at given index
    private int GetRowOfIndex(int indexCell)
    {
        throw new NotImplementedException();
    }
    //returns the column at given index
    private int GetColumnOfIndex(int indexCell)
    {
        throw new NotImplementedException();
    }
    
    //Given a position (as a Vector3 in world space), it returns the cell closest to the position. 
    private int GetGridIndex(Vector3 position)
    {
        throw new NotImplementedException();
    }
}
