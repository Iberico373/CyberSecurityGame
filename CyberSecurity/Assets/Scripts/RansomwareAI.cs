using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RansomwareAI : BaseAI
{
    private void Awake()
    {
        manager = UnitManager.instance;
        anim = GetComponent<Animator>();
        startTurn = true;

        GetAggroList();
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
                ds.GetComponent<DataStructure>().isLocked = false;
            }
        }

        if (isDetected)
        {

            aura.SetActive(false);
            if (SceneManager.GetActiveScene().name == "TestLevel")
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

    public override void Action()
    {
        if (target.CompareTag("Security Control") || target.name.Equals("Objective"))
        {
            transform.LookAt(target.transform);
            anim.SetTrigger("Attack");
            target.GetComponent<Unit>().health -= 5;

            if (target.GetComponent<Unit>().health <= 0)
            {
                target.GetComponent<Unit>().anim.SetTrigger("Dead");
                NextUnit();
            }
        }

        else if (target.name.Equals("Data Structure"))
        {
            target.GetComponent<DataStructure>().isLocked = true;
            infectedDS.Add(target);
            NextUnit();
        }
    }
}
