using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Object : MonoBehaviour
{
    public GameObject heal;
    public GameObject cleanse;

    private int healcount = 0;
    private int cleansecount = 0;
    void Start()
    {
        heal = transform.Find("Heal").gameObject;
        cleanse = transform.Find("Cleanse").gameObject;
    }

    public void Healing()
    {
        healcount++;
        heal.GetComponent<TMPro.TextMeshProUGUI>().text = "Use recovery control to heal 2 security controls (" + healcount.ToString() + "/2)";
    }

    public void Cleansing()
    {
        cleansecount++;
        cleanse.GetComponent<TMPro.TextMeshProUGUI>().text = "Use corrective control to cleanse 2 effects (" + cleansecount.ToString() + "/2)";
    }
    
}
