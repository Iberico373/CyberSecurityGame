using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTutorial : MonoBehaviour
{    
    public GameObject scan;
    public GameObject action;
    public GameObject attackpan;
    
    bool firstAction;
    bool firstScan;
    bool firstAttack;

    private void Start()
    {
        firstAction = true;
        firstScan = true;
        firstAttack = true;
    }

    public void SetTutorial(int state)
    {
        if (state == 0)
        {
            if (firstAction)
            {
                action.SetActive(true);
                firstAction = false;
            }
        }

        else if (state == 1)
        {
            if (firstScan)
            {
                scan.SetActive(true);
                firstScan = false;
            }
        }
        else if (state == 2)
        {
            if (firstAttack)
            {
                attackpan.SetActive(true);
                firstAttack = false;
            }
        }
    }

    
}
