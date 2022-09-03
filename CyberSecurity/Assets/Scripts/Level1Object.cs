using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Object : MonoBehaviour
{
    public GameObject heal;
    public GameObject healcomp;
    public GameObject push;
    public GameObject pushcomp;
    public GameObject kill;
    private int killcount = 0;


    void Start()
    {
        heal = transform.Find("Heal").gameObject;
        healcomp = transform.Find("Healcomp").gameObject;
        push = transform.Find("Push").gameObject;
        pushcomp = transform.Find("Pushcomp").gameObject;
        kill = transform.Find("Kill").gameObject;

        healcomp.SetActive(false);
        pushcomp.SetActive(false);
      
    }

    public void Killing()
    {
        killcount++;
        kill.GetComponent<TMPro.TextMeshProUGUI>().text = "Defeat all malwares (" + killcount.ToString() +"/3)";
    }
}
