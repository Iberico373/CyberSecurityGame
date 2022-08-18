using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    None,
    Preventative,
    Detective,
    Deterrent,
    Virus,
    Bot
}

public class DataStructure : Unit
{
    public bool isLocked;
    public State _currentState;
    public Renderer render;
    public GameObject bot;

    UnitManager manager;
    HashSet<Node> nodesInRange;
    List<Node> adjacentNodes;
    int deterrentCD = 0;
    int botCD = 0;

    //Visual Effects for attacks
    public GameObject laser;
    public GameObject scan;

    private void Awake()
    {
        manager = UnitManager.instance;
        nodesInRange = manager.pathfinding.MovementRadius(transform.position);
        adjacentNodes = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(transform.position), 1);

    }

    private void Update()
    {
        if(isLocked)
        {
            aura.SetActive(true);
        }
        else
        {
            aura.SetActive(false);
            if(_currentState == State.Detective)
            {
                render.material.color = new Color(0, 20, 255, 100);
            }
            if (_currentState == State.Preventative)
            {
                render.material.color = new Color(255, 0, 0, 100);
            }
            if(_currentState == State.None)
            {
                render.material.color = new Color(255, 255, 255, 100);
            }
        }
    }
    public void StateEffect()
    {
        switch (_currentState)
        {
            case State.None:
                
                break;

            case State.Preventative:
                if (isLocked)
                {
                    return;
                }

                GameObject summonedLaser = Instantiate(laser);
                summonedLaser.transform.position = transform.position;
                Destroy(summonedLaser, 5);
                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() != null)
                    {
                        if (n.ReturnObject().CompareTag("Malware"))
                        {
                            if(n.ReturnObject().GetComponent<Unit>().isDetected)
                            {
                                n.ReturnObject().GetComponent<Unit>().health -= 5;
                            }
                        }
                    }
                }
                break;

            case State.Detective:
                if (isLocked)
                {
                    return;
                }

                GameObject summonedScan = Instantiate(scan);
                summonedScan.transform.position = transform.position;
                Destroy(summonedScan, 4);
                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() != null)
                    {
                        if (n.ReturnObject().CompareTag("Malware"))
                        {
                            n.ReturnObject().GetComponent<Unit>().isDetected = true;
                        }
                    }
                }
                break;

            case State.Deterrent:
                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() != null)
                    {
                        if (n.ReturnObject().CompareTag("Malware"))
                        {
                            if (deterrentCD == 0)
                            {
                                n.ReturnObject().GetComponent<Unit>().isStunned = true;
                                deterrentCD++;
                            }

                            else
                            {
                                if (deterrentCD == 3)
                                {
                                    deterrentCD = 0;
                                    break;
                                }

                                deterrentCD++;
                            }
                        }
                    }
                }
                break;

            case State.Virus:
                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() != null)
                    {
                        if (n.ReturnObject().CompareTag("Security Control"))
                        {
                            if (n.ReturnObject().GetComponent<Unit>().isCorrupted < 5)
                            {
                                n.ReturnObject().GetComponent<Unit>().isCorrupted++;
                            }
                        }
                    }
                }
                break;

            case State.Bot:
                foreach (Node n in adjacentNodes)
                {
                    if (n.ReturnObject() == null && botCD == 0)
                    {
                        Instantiate(bot, n.worldPos, Quaternion.identity);
                        botCD++;
                        break;
                    }

                    else if (botCD > 0)
                    {
                        if (botCD == 3)
                        {
                            botCD = 0;
                            break;
                        }

                        botCD++;
                    }
                }
                break;

            default:
                break;
        }
    }
}
