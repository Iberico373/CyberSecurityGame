using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recover", menuName = "Effect/Recover")]
public class Recover : Effect 
{
    public int Health;

    public override void UseEffect()
    {
        Debug.Log("TEST");
        Unit[] units = FindObjectsOfType<Unit>();
        for(int i = 0; i < units.Length; i++)
        {
            units[i].health += Health;
        }
    }
}
