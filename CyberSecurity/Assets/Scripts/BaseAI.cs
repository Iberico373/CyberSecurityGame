using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (unit.gameObject.name.Equals("Data Structure"))
            {
                if (unit.GetComponent<DataStructure>().capturedM)
                {
                    continue;
                }

                aggrolist.Add(unit.gameObject);
            }
        }

        aggrolist.Add(GameObject.Find("Objective"));
    }

    public void SortAggroListByDistance()
    {
        GameObject temp;
        for (int j = 0; j <= aggrolist.Count - 2; j++)
        {
            for (int i = 0; i <= aggrolist.Count - 2; i++)
            {
                if (manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), 
                    manager.grid.NodeFromWorldPoint(aggrolist[i].transform.position)) >
                    manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), 
                    manager.grid.NodeFromWorldPoint(aggrolist[i + 1].transform.position)))
                {
                    temp = aggrolist[i + 1];
                    aggrolist[i + 1] = aggrolist[i];
                    aggrolist[i] = temp;
                }
            }
        }
    }


    public void SelectTarget()
    {
        target = aggrolist[0];

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
        Vector3 nearestTile = new Vector3();

        while (true)
        {
            List<Node> AdjacentTiles = manager.grid.GetNeighbours(targetNode, 1);
            Node nearestNode = AdjacentTiles[0];
            List<Node> tempOpen = new List<Node>();

            foreach (Node n in AdjacentTiles)
            {
                if (n.ReturnObject() == null)
                {
                    tempOpen.Add(n);
                }

                nearestNode = CompareNodeDist(nearestNode, n);
            }

            if (nearestNode.ReturnObject() != null)
            {
                if (tempOpen != null)
                {
                    nearestNode = tempOpen[0];

                    foreach (Node m in tempOpen)
                    {
                        nearestNode = CompareNodeDist(nearestNode, m);
                    }

                    nearestTile = nearestNode.worldPos;
                    break;
                }

                else if (nearestNode.ReturnObject().CompareTag("Security Control"))
                {
                    targetNode = nearestNode;
                    aggrolist.Add(nearestNode.ReturnObject());
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
        if (manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), nodeA)
            > manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), nodeB))
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
        aggrolist.Insert(aggrolist.Count, temp);

        target = aggrolist[0];
        targetPos = GetNearestTile(manager.grid.NodeFromWorldPoint(target.transform.position));
    }

    public virtual void Action()
    {

    }
}
