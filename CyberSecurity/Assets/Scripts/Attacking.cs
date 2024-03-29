using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public Attack target;
    public GameObject strike;
    UnitManager manager;
    public void Attackin()
    {
        manager = UnitManager.instance;
        strike = target.target;

        if (strike.GetComponent<Unit>().isDetected)
        {
            if (manager.selectedCharacter.buffed > 0)
            {
                manager.selectedCharacter.buffed -= 1;
                strike.GetComponent<Unit>().health -= 20;
                Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " did 20 damage to ", strike.name);
            }

            else
            {
                strike.GetComponent<Unit>().health -= 10;
                manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " did 10 damage to ", strike.name);
            }

            strike.GetComponent<BaseAI>().aggrolist.Remove(manager.selectedCharacter.gameObject);
            strike.GetComponent<BaseAI>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);
        }
    }
}
