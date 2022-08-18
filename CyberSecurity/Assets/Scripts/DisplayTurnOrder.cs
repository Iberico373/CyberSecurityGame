using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTurnOrder : MonoBehaviour
{
    UnitManager manager;

    private void Awake()
    {
        manager = UnitManager.instance;

        if (manager.unitList.Count < 6)
        {
            for (int i = 0; i < manager.unitList.Count; i++)
            {
                if (manager.unitList[i].turnOrderDisplay == null)
                {
                    continue;
                }

                Instantiate(manager.unitList[i].turnOrderDisplay, transform);
            }
        }

        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (manager.unitList[i].turnOrderDisplay == null)
                {
                    continue;
                }

                Instantiate(manager.unitList[i].turnOrderDisplay, transform);
            }
        }
    }

    public void UpdateTurnOrder()
    {
        //transform.GetChild(0).SetAsLastSibling();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //if (manager.unitList.Count < 6)
        //{
        //    for (int i = 0; i < manager.unitList.Count; i++)
        //    {
        //        if (manager.unitList[i].turnOrderDisplay == null)
        //        {
        //            continue;
        //        }

        //        Instantiate(manager.unitList[i].turnOrderDisplay, transform);
        //    }
        //}

        for (int i = 0; i < manager.unitList.Count; i++)
        {
            if (manager.unitList[i].turnOrderDisplay == null)
            {
                continue;
            }

            Instantiate(manager.unitList[i].turnOrderDisplay, transform);
        }
    }
}
