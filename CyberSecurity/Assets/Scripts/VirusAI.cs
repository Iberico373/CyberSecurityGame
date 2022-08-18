using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VirusAI : Unit
{
    UnitManager manager;
    GameObject target;
    Vector3 targetPos;
    List<GameObject> infectedDS = new List<GameObject>();
    List<GameObject> infectedSC = new List<GameObject>();

    private void Awake()
    {
        manager = UnitManager.instance;
        anim = GetComponent<Animator>();
        startTurn = true;

        foreach (Unit unit in manager.unitList)
        {
            if (!unit.gameObject.CompareTag("Malware"))
            {
                aggrolist.Add(unit.gameObject);
            }
        }

        aggrolist.Add(GameObject.Find("Objective"));

        target = aggrolist[0];
        targetPos = GetNearestTile(manager.grid.NodeFromWorldPoint(target.transform.position));
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);

            foreach (GameObject ds in infectedDS)
            {
                ds.GetComponent<DataStructure>().isCorrupted = 0;
            }
            foreach (GameObject sc in infectedSC)
            {
                sc.GetComponent<Unit>().isCorrupted = 0;
            }
        }
        if (isDetected)
        {
            aura.SetActive(false);
            if(SceneManager.GetActiveScene().name == "TestLevel")
            {
                scanobj.SetActive(false);
                scancomplete.SetActive(true);
            }
            
        }

        if (!startTurn && manager.selectedCharacter.name.Equals(transform.name))
        {
            if (InRange())
            {
                Action();
            }

            startTurn = true;
            manager.EndTurn();
        }
    }

    public void SelectTarget()
    {
        //CheckAggroList();
        SortAggroListByDistance();
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

    Vector3 GetNearestTile(Node targetNode)
    {
        Vector3 nearestTile = new Vector3();
        //int currentDistance = manager.pathfinding.
        //    GetDistance(manager.grid.NodeFromWorldPoint(transform.position),
        //    AdjacentTiles[0]);       

        while (true)
        {
            List<Node> AdjacentTiles = manager.grid.GetNeighbours(targetNode, 1);
            Node nearestNode = AdjacentTiles[0];
            List<Node> tempOpen = new List<Node>();

            foreach (Node n in AdjacentTiles)
            {
                //currentDistance = manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), n);

                //if (currentDistance > manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), n))
                //{
                //    currentDistance = manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), n);
                //    nearestTile = n.worldPos;
                //}

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

    Node CompareNodeDist(Node nodeA, Node nodeB)
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

    bool InRange()
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

    void Action()
    {
        if (target.CompareTag("Security Control") || target.name.Equals("Objective"))
        {
            transform.LookAt(target.transform);
            anim.SetTrigger("Attack");
            target.GetComponent<Unit>().health -= 3;
            if (target.GetComponent<Unit>().isCorrupted < 5)
            {
                target.GetComponent<Unit>().isCorrupted++;
            }
            infectedSC.Add(target);
            if (target.GetComponent<Unit>().health <= 0)
            {
                target.GetComponent<Unit>().anim.SetTrigger("Dead");
                NextUnit();
            }
        }

        else if (target.name.Equals("Data Structure"))
        {
            if (target.GetComponent<DataStructure>().isCorrupted < 5)
            {
                target.GetComponent<DataStructure>().isCorrupted++;
            }
            infectedDS.Add(target);
            NextUnit();
        }
    }

    void CheckAggroList()
    {
        if (aggrolist[0].CompareTag("Security Control"))
        {
            target = aggrolist[0];

            //if (InRange())
            //{
            //    Action();
            //    manager.EndTurn();
            //}

            //else
            //{
            //    targetPos = GetNearestTile(manager.grid.NodeFromWorldPoint(target.transform.position));
            //}
        }
    }

    void NextUnit()
    {
        GameObject temp = aggrolist[0];
        aggrolist.Remove(aggrolist[0]);
        aggrolist.Insert(aggrolist.Count, temp);

        target = aggrolist[0];
        targetPos = GetNearestTile(manager.grid.NodeFromWorldPoint(target.transform.position));
    }

    void SortAggroListByDistance()
    {
        GameObject temp;
        for (int j = 0; j <= aggrolist.Count - 2; j++)
        {
            for (int i = 0; i <= aggrolist.Count - 2; i++)
            {
                if (manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), manager.grid.NodeFromWorldPoint(aggrolist[i].transform.position)) >
                    manager.pathfinding.GetDistance(manager.grid.NodeFromWorldPoint(transform.position), manager.grid.NodeFromWorldPoint(aggrolist[i + 1].transform.position)))
                {
                    temp = aggrolist[i + 1];
                    aggrolist[i + 1] = aggrolist[i];
                    aggrolist[i] = temp;
                }
            }
        }
    }
}
