using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Object : MonoBehaviour
{
    public GameObject upgrade;
    public GameObject upgradecomp;
    public GameObject cleanse;
    public GameObject cleansecomp;
    void Start()
    {
        upgrade = transform.Find("Upgrade").gameObject;
        upgradecomp = transform.Find("Upgradecomp").gameObject;
        cleanse = transform.Find("Cleanse").gameObject;
        cleansecomp = transform.Find("Cleansecomp").gameObject;

        upgradecomp.SetActive(false);
        cleansecomp.SetActive(false);
  
    }
}
