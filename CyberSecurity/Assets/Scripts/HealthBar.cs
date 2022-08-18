using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    int hp;
    public Unit unit;
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        hp = unit.health;
        healthBar.maxValue = hp;
        healthBar.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        hp = unit.health;
        healthBar.value = hp;
    }
}
