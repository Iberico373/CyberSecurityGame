using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : Unit
{
    public UnitManager manager;
    public GameObject target;
    public Vector3 targetPos;
    public List<GameObject> aggrolist;
    public List<GameObject> infectedDS = new List<GameObject>();

    private void Start()
    {
        isDetected = false;
    }

    public void GetAggroList()
    {
        foreach (Unit unit in manager.unitList)
        {
            if (!unit.CompareTag("Malware"))
            {
                aggrolist.Add(unit.gameObject);
            }
        }
    }

    public void SortAggroListByUnit()
    {
        GameObject temp;

        for (int i = 0; i < aggrolist.Count; i++)
        {
            if (aggrolist[0].CompareTag("Security Control"))
            {
                temp = aggrolist[0];
                aggrolist.Remove(aggrolist[0]);
                aggrolist.Add(temp);
            }
        }
    }

    public void SortAggroListByDistance()
    {
        GameObject temp;
        Node startNode = manager.grid.NodeFromWorldPoint(transform.position);

        for (int j = 0; j <= aggrolist.Count - 2; j++)
        {
            for (int i = 0; i <= aggrolist.Count - 2; i++)
            {
                if (manager.pathfinding.GetDistance(startNode, manager.grid.NodeFromWorldPoint(aggrolist[i].transform.position)) >
                    manager.pathfinding.GetDistance(startNode, manager.grid.NodeFromWorldPoint(aggrolist[i + 1].transform.position)))
                {
                    temp = aggrolist[i + 1];
                    aggrolist[i + 1] = aggrolist[i];
                    aggrolist[i] = temp;
                }
            }
        }
    }

    public void UpdateAggroList()
    {
        GameObject temp;

        for (int i = 0; i < aggrolist.Count; i++)
        {
            if (aggrolist[i].CompareTag("Security Control"))
            {
                if (aggrolist[i].GetComponent<Unit>().health <= 0)
                {
                    temp = aggrolist[i];

                    aggrolist.Remove(aggrolist[i]);
                    aggrolist.Add(temp);
                }
            }

            else if (aggrolist[i].GetComponent<Unit>().id == 7)
            {
                if (aggrolist[i].GetComponent<DataStructure>().capturedSC)
                {
                    if (aggrolist[0].CompareTag("Security Control"))
                    {
                        Node nodeA = manager.grid.NodeFromWorldPoint(aggrolist[i].transform.position);
                        Node nodeB = manager.grid.NodeFromWorldPoint(aggrolist[0].transform.position);
                        temp = aggrolist[i];

                        if (CompareNodeDist(nodeA, nodeB) == nodeA)
                        {                           
                            aggrolist.Remove(aggrolist[i]);
                            aggrolist.Insert(0, temp);
                        }

                        else
                        {
                            aggrolist.Remove(aggrolist[i]);
                            aggrolist.Insert(1, temp);
                        }
                    }
                    
                    else if (aggrolist[0].GetComponent<Unit>().id == 7)
                    {
                        Node nodeA = manager.grid.NodeFromWorldPoint(aggrolist[i].transform.position);
                        Node nodeB = manager.grid.NodeFromWorldPoint(aggrolist[0].transform.position);
                        temp = aggrolist[i];

                        if (CompareNodeDist(nodeA, nodeB) == nodeA)
                        {
                            aggrolist.Remove(aggrolist[i]);
                            aggrolist.Insert(0, temp);
                        }

                        else
                        {
                            aggrolist.Remove(aggrolist[i]);
                            aggrolist.Insert(1, temp);
                        }
                    }
                }

                else if (aggrolist[i].GetComponent<DataStructure>().capturedM)
                {
                    if (!aggrolist[i].GetComponent<DataStructure>().isLocked 
                        && manager.selectedCharacter.GetComponent<Unit>().id == 4)
                    {
                        return;
                    }

                    temp = aggrolist[i];

                    aggrolist.Remove(aggrolist[i]);
                    aggrolist.Add(temp);
                }
            }
        }
    }

    public void SelectTarget()
    {
        UpdateAggroList();

        target = aggrolist[0];

        if (target == null)
        {
            manager.EndTurn();
        }

        if (InRange())
        {
            Action();
            manager.EndTurn();
        }

        else
        {
            targetPos = GetNearestTile(manager.grid.NodeFromWorldPoint(target.transform.position));
            Move(targetPos);
        }
    }

    public Vector3 GetNearestTile(Node targetNode)
    {
        manager.grid.UpdateGrid();
        Vector3 nearestTile = new Vector3();

        while (true)
        {
            List<Node> adjacentTiles = manager.grid.GetNeighbours(targetNode, 1);
            List<Node> previousNodes = new List<Node>();
            Node openNode = null;
            Node nearestNode = adjacentTiles[0];

            foreach (Node n in adjacentTiles)
            {
                if (previousNodes.Contains(n))
                {
                    continue;
                }

                if (n.walkable)
                {
                    if (openNode == null)
                    {
                        openNode = n;
                    }

                    else
                    {
                        openNode = CompareNodeDist(openNode, n);
                    }
                }

                previousNodes.Add(n);
                nearestNode = CompareNodeDist(nearestNode, n);
            }

            if (nearestNode.ReturnObject() != null)
            {
                if (openNode != null)
                {
                    nearestTile = openNode.worldPos;
                    break;
                }

                else if (nearestNode.ReturnObject().CompareTag("Security Control"))
                {
                    targetNode = nearestNode;
                    aggrolist.Insert(0, nearestNode.ReturnObject());
                }

                else if (nearestNode.ReturnObject().CompareTag("Malware"))
                {
                    targetNode = nearestNode;
                }
            }

            else
            {
                nearestTile = nearestNode.worldPos;
                break;
            }
        }

        return nearestTile;
    }

    public Node CompareNodeDist(Node nodeA, Node nodeB)
    {
        Node startNode = manager.grid.NodeFromWorldPoint(transform.position);

        if (manager.pathfinding.GetDistance(startNode, nodeA) > manager.pathfinding.GetDistance(startNode, nodeB))
        {
            return nodeB;
        }

        else
        {
            return nodeA;
        }
    }

    public bool InRange()
    {
        List<Node> tiles = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(manager.selectedCharacter.transform.position), 1);

        foreach (Node n in tiles)
        {
            if (n.ReturnObject() != null)
            {
                if (n.ReturnObject() == target)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void NextUnit()
    {
        GameObject temp = aggrolist[0];
        aggrolist.Remove(aggrolist[0]);
        aggrolist.Add(temp);
    }

    public virtual void Action()
    {

    }
}
