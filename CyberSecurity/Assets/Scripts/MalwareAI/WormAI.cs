using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WormAI : BaseAI
{
    List<GameObject> infectedSC = new List<GameObject>();
    List<Node> nodesInRange;
    int pulseCooldown;
    private void Awake()
    {
        manager = UnitManager.instance;
        anim = GetComponent<Animator>();
        startTurn = true;
        GetAggroList();
        SortAggroListByDistance();
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (SceneManager.GetActiveScene().name == "TestLevel")
            {
                UnitManager.instance.objectives.GetComponent<TutorialObject>().kill.SetActive(false);
                UnitManager.instance.objectives.GetComponent<TutorialObject>().killcomp.SetActive(true);
            }

            else if (SceneManager.GetActiveScene().name == "Level1")
            {
                UnitManager.instance.objectives.GetComponent<Level1Object>().Killing();
            }

            manager.unitList.Remove(this);

            foreach (GameObject ds in infectedDS)
            {
                ds.GetComponent<DataStructure>()._currentState = State.None;
            }

            Destroy(gameObject);
        }

        if (isDetected)
        {
            aura.SetActive(false);

            if (SceneManager.GetActiveScene().name == "TestLevel")
            {
                manager.objectives.GetComponent<TutorialObject>().scancomp.SetActive(true);
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

    public override void Action()
    {
        nodesInRange = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(transform.position), 1);
        if(pulseCooldown <= 0)
        {
            pulseCooldown = 2;
            foreach (Node n in nodesInRange)
            {
                if (n.ReturnObject() != null)
                {
                    if (n.ReturnObject().CompareTag("Security Control"))
                    {
                        if (!n.ReturnObject().GetComponent<Unit>().isStunned)
                        {
                            n.ReturnObject().GetComponent<Unit>().isStunned = true;
                        }
                    }
                }
            }
        }
        else
        {
            pulseCooldown--;
        }
        
        if (target.CompareTag("Security Control") || target.name.Equals("Objective"))
        {
            transform.LookAt(target.transform);
            anim.SetTrigger("Attack");
            target.GetComponent<Unit>().health -= 8;

            if (target.GetComponent<Unit>().health <= 0)
            {
                target.GetComponent<Unit>().anim.SetTrigger("Dead");
                NextUnit();
            }
        }

        else if (target.name.Equals("Data Structure"))
        {
            target.GetComponent<DataStructure>()._currentState = State.Worm;
            infectedDS.Add(target);
            NextUnit();
        }
    }
}
