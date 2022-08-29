using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager request;
    MyGrid grid;
    //Starting node of the AI
    Node startNode;
    //The node the AI is targetting
    Node targetNode;
    //Array containing the waypoints the AI will follow
    Vector3[] waypoints;
    //Determines if a path is successful or not
    bool pathSuccess;

    //Amount of tiles the unit can move
    public int radius = 2;

    private void Awake()
    {
        request = GetComponent<PathRequestManager>();
        grid = GameObject.Find("Grid").GetComponent<MyGrid>();
    }

    //Displays movement radius of a selected unit
    public HashSet<Node> MovementRadius(Vector3 startPos, bool ignoreObstacle)
    {
        grid.ClearGrid();
        grid.UpdateGrid();

        Node startingNode = grid.NodeFromWorldPoint(startPos);

        //Set containing all the furthest highlighted tiles
        HashSet<Node> ui = new HashSet<Node>();
        //Set containing all the tiles surrounding the furthest highlighted tiles
        HashSet<Node> tempUi = new HashSet<Node>();
        //Set containing all the highlighted tiles
        HashSet<Node> final = new HashSet<Node>();

        final.Add(startingNode);

        //For every neighbouring node surrounding the starting node...
        foreach (Node neighbour in grid.GetNeighbours(startingNode, 1))
        {
            if (!ignoreObstacle)
            {
                if (radius - neighbour.cost >= 0)
                {
                    neighbour.cost = 1;
                    //Add neighbour to ui
                    ui.Add(neighbour);
                }
            }

            else
            {
                neighbour.cost = 1;
                ui.Add(neighbour);
            }
            
        }

        final.UnionWith(ui);

        while (ui.Count > 0)
        {
            foreach (Node n in ui)
            {
                foreach (Node neighbour in grid.GetNeighbours(n, 1))
                {
                    if (!final.Contains(neighbour))
                    {
                        if (tempUi.Contains(neighbour))
                        {
                            neighbour.cost = 1;
                        }

                        if (neighbour.cost == Mathf.Infinity && ignoreObstacle)
                        {
                            neighbour.cost = 1;
                        }

                        neighbour.cost = neighbour.cost + n.cost;

                        if (radius - neighbour.cost >= 0)
                        {
                            tempUi.Add(neighbour);
                        }
                    }
                }
            }

            ui = tempUi;
            final.UnionWith(ui);
            tempUi = new HashSet<Node>();
        }

        return final;
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //Finds the shortest path from the starting position and the target position
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid.ClearGrid();
        grid.UpdateGrid();

        waypoints = new Vector3[0];
        startNode = grid.NodeFromWorldPoint(startPos);
        targetNode = grid.NodeFromWorldPoint(targetPos);

        //A set containing all the open nodes 
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        //A set containing all the closed nodes 
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        //While there is an open node...
        while (openSet.Count > 0)
        {
            //Get the current node by getting the first node from the heap
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            //If the pathfinding has reached its destination, call RetracePath()
            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            //For every neighbour around the current node...
            foreach (Node neighbour in grid.GetNeighbours(currentNode, 1))
            {
                //If the neighbouring node is not walkable or closed, continue
                //to the next neighbour
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int moveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                //If the movement cost to neighbour is less than the g cost of the neighbour or the neighbour is not open...
                if (moveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //Set the neighbour's 'gCost' to the move cost to neighbour
                    neighbour.gCost = moveCostToNeighbour;
                    //Calculate the 'hCost' by getting the distance between the neighbouring node and the target node
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    //Set the neighbour's parent to be the current node
                    neighbour.parent = currentNode;

                    //If the neighbour is not open, set it to be open
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }

                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;

        //If a succesful path has been found, store all the nodes the pathfinding took inside waypoints
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        request.FinishedProcessing(waypoints, pathSuccess);
    }

    //Gets the previous nodes and stores them in a list called 'path'
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        //While the current node is not the starting node, add the current node inside path list
        //and get the parent of the current node
        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPos);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = path.ToArray();
        //Reverse the waypoints array so that it shows the path from start to end 
        Array.Reverse(waypoints);
        return waypoints;
    }

    //Simplifies path so that it only has waypoints when the path changes directions
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
                directionOld = directionNew;
            }
        }

        return waypoints.ToArray();
    }

    //Gets the distance between the current node and the neighbouring node
    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}

