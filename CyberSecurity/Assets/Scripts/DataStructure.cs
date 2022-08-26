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
    Bot,
    Worm
}

public class DataStructure : Unit
{
    public bool isLocked;
    public bool capturedM = false;
    public bool capturedSC = false;
    public State _currentState;
    public Renderer render;
    public GameObject bot;

    UnitManager manager;
    HashSet<Node> nodesInRange;
    List<Node> adjacentNodes;
    int deterrentCD = 0;
    int botCD = 0;
    int wormCD = 0;

    //Visual Effects for attacks
    public GameObject laser;
    public GameObject scan;
    public GameObject stun;
    public GameObject chain;

    private void Awake()
    {
        manager = UnitManager.instance;
        nodesInRange = manager.pathfinding.MovementRadius(transform.position, true);
        adjacentNodes = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(transform.position), 1);

    }

    private void Update()
    {
        if (_currentState == State.None)
        {
            chain.SetActive(false);
            aura.SetActive(false);
            render.material.color = new Color(255, 255, 255, 100);
        }


        else if (_currentState == State.Detective)
        {
            capturedM = false;
            capturedSC = true;

            chain.SetActive(false);
            aura.SetActive(false);
            render.material.color = new Color(0, 20, 255, 100);
        }

        else if (_currentState == State.Preventative)
        {
            capturedM = false;
            capturedSC = true;

            chain.SetActive(false);
            aura.SetActive(false);
            render.material.color = new Color(255, 0, 0, 100);
        }



        else if (_currentState == State.Deterrent)
        {
            capturedM = false;
            capturedSC = true;

            chain.SetActive(false);
            aura.SetActive(false);
            render.material.color = new Color(0, 0, 50, 100);
        }

        else if (_currentState == State.Bot)
        {
            capturedM = true;
            capturedSC = false;

            if (isLocked)
            {
                chain.SetActive(true);
            }

            aura.SetActive(true);
            render.material.color = new Color(20, 0, 200);
        }

        else if (_currentState == State.Virus)
        {
            capturedM = true;
            capturedSC = false;

            if (isLocked)
            {
                chain.SetActive(true);
            }

            aura.SetActive(true);
            render.material.color = new Color(20, 0, 200);
        }

        else if (_currentState == State.Worm)
        {
            capturedM = true;
            capturedSC = false;

            if (isLocked)
            {
                chain.SetActive(true);
            }

            aura.SetActive(true);
            render.material.color = new Color(20, 0, 200);
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
                if (isLocked)
                {
                    return;
                }

                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() != null)
                    {
                        if (n.ReturnObject().CompareTag("Malware"))
                        {
                            if (deterrentCD == 0)
                            {
                                GameObject summonedStun = Instantiate(stun);
                                summonedStun.transform.position = transform.position;
                                Destroy(summonedStun, 5);
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
                        GameObject botClone = Instantiate(bot, manager.unitGroup.transform);
                        botClone.transform.position = n.worldPos;
                        botClone.layer = 3;
                        manager.SortTurnOrder();

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

            case State.Worm:
                capturedSC = false;
                capturedM = true;

                foreach (Node n in adjacentNodes)
                {
                    if (n.ReturnObject() != null && wormCD == 0)
                    {
                        if (n.ReturnObject().CompareTag("Security Control"))
                        {
                                n.ReturnObject().GetComponent<Unit>().health -= 10;
                        }
                    }

                    else if (wormCD > 0)
                    {
                        if (wormCD == 2)
                        {
                            wormCD = 0;
                            break;
                        }

                        wormCD++;
                    }
                }
                break;
            default:
                break;
        }
    }
}
