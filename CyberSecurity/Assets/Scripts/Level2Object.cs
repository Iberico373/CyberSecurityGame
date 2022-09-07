using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Object : MonoBehaviour
{
    public GameObject upgrade;
    public GameObject upgradecomp;
    public GameObject cleanse;
    public GameObject cleansecomp;
    public GameObject kill;
    private int killcount = 0;
    void Start()
    {
        upgrade = transform.Find("Upgrade").gameObject;
        upgradecomp = transform.Find("Upgradecomp").gameObject;
        cleanse = transform.Find("Cleanse").gameObject;
        cleansecomp = transform.Find("Cleansecomp").gameObject;
        kill = transform.Find("Kill").gameObject;

        upgradecomp.SetActive(false);
        cleansecomp.SetActive(false);
  
    }

    public void Killing()
    {
        killcount++;
        kill.GetComponent<TMPro.TextMeshProUGUI>().text = "Defeat 5 malwares (" + killcount.ToString() + "/5)";
    }
}
