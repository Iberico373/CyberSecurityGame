using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    //Determines if the node is walkable or not
    public bool walkable;
    //World position of the node
    public Vector3 worldPos;
    //Tile associated with the node
    public GameObject tile;
    //x position of the node relative to the grid
    public int gridX;
    //y position of the node relative to the grid
    public int gridY;
    //Movement cost of a tile
    public float cost;

    //Distance from starting node
    public int gCost;
    //Distance from the target node
    public int hCost;
    //Parent of the node
    public Node parent;

    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPos, GameObject _tile, int _gridX, int _gridY, int _cost)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        tile = _tile;
        gridX = _gridX;
        gridY = _gridY;
        cost = _cost;
    }

    public int fCost { get { return gCost + hCost; } }

    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }

    public GameObject ReturnObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(tile.transform.position, Vector3.up * 10, out hit))
        {
            return hit.collider.gameObject;
        }

        else
            return null;
    }
}

