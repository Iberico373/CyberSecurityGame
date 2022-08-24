using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusAI : BaseAI
{
    List<GameObject> infectedSC = new List<GameObject>();

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

            if(SceneManager.GetActiveScene().name == "TestLevel")
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
        if (target.CompareTag("Security Control") || target.name.Equals("Objective"))
        {
            transform.LookAt(target.transform);
            anim.SetTrigger("Attack");
            target.GetComponent<Unit>().health -= 5;

            if (target.GetComponent<Unit>().isCorrupted < 5)
            {
                if (target.GetComponent<Unit>().isCorrupted == 0)
                {
                    infectedSC.Add(target);
                }

                target.GetComponent<Unit>().isCorrupted++;                
            }

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
}
