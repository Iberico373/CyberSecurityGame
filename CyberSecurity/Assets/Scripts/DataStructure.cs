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
        if (isLocked)
        {
            aura.SetActive(true);
            chain.SetActive(true);
        }
        else
        {
            
            if (_currentState == State.Detective)
            {
                chain.SetActive(false);
                aura.SetActive(false);
                render.material.color = new Color(0, 20, 255, 100);
            }
            if (_currentState == State.Preventative)
            {
                chain.SetActive(false);
                aura.SetActive(false);
                render.material.color = new Color(255, 0, 0, 100);
            }
            if (_currentState == State.None)
            {
                chain.SetActive(false);
                aura.SetActive(false);
                render.material.color = new Color(255, 255, 255, 100);
            }
            if (_currentState == State.Deterrent)
            {
                chain.SetActive(false);
                aura.SetActive(false);
                render.material.color = new Color(0, 0, 50, 100);
            }
            if (_currentState == State.Bot)
            {
                aura.SetActive(true);
                render.material.color = new Color(20, 0, 200);
            }
            if(_currentState == State.Virus)
            {
                aura.SetActive(true);
                render.material.color = new Color(20, 0, 200);
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

                capturedSC = true;
                capturedM = false;

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

                capturedSC = true;
                capturedM = false;

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

                capturedSC = true;
                capturedM = false;

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
                capturedSC = false;
                capturedM = true;

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
                capturedSC = false;
                capturedM = true;

                foreach (Node n in adjacentNodes)
                {
                    if (n.ReturnObject() == null && botCD == 0)
                    {
                        GameObject botClone = Instantiate(bot, manager.unitGroup.transform);
                        botClone.transform.position = n.worldPos;
                        botClone.layer = 3;
                        botClone.GetComponent<BotAI>().GetAggroList();
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

            default:
                break;
        }
    }
}
