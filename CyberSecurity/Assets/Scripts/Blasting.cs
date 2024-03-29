using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blasting : MonoBehaviour
{
    public Blast target;
    public GameObject strike;
    public GameObject waves;
    UnitManager manager;

    public void Blastin()
    {
        manager = UnitManager.instance;
        strike = target.target;

        if (manager.selectedCharacter.buffed > 0)
        {
            strike.GetComponent<Unit>().stun += 2;
            strike.GetComponent<Unit>().stunEffect.SetActive(true);
        }

        Vector3 dir = (strike.transform.position - manager.selectedCharacter.transform.position).normalized;

        GameObject summonedWave = Instantiate(waves,transform);
        summonedWave.transform.position = transform.position + new Vector3(0,3,0) + transform.forward * 3;
        strike.transform.position = strike.transform.position + dir * manager.grid.nodeRadius * 2;
        manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " pushed back ", strike.name);
        manager.selectedCharacter.inAction = false;

        strike.GetComponent<BaseAI>().aggrolist.Remove(manager.selectedCharacter.gameObject);
        strike.GetComponent<BaseAI>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);
    }
}
