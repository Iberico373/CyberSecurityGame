using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blasting : MonoBehaviour
{
    public Blast target;
    public GameObject strike;
    UnitManager manager;
    public void Blastin()
    {
        manager = UnitManager.instance;
        strike = target.target;
        Vector3 dir = (strike.transform.position - manager.selectedCharacter.transform.position).normalized;
<<<<<<< Updated upstream
=======
        GameObject summonedWave = Instantiate(waves,transform);
        summonedWave.transform.position = transform.position + new Vector3(0,3,0) + transform.forward * 3;
        strike.GetComponent<BaseAI>().anim.SetTrigger("Hurt");
>>>>>>> Stashed changes
        strike.transform.position = strike.transform.position + dir * manager.grid.nodeRadius * 2;
        strike.GetComponent<BaseAI>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);
        manager.grid.UpdateGrid();
    }
    
}
