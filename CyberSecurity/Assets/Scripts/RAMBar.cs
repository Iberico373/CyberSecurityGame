using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RAMBar : MonoBehaviour
{
    public Deck deck;
    public Slider ramBar;
    // Start is called before the first frame update
    void Start()
    {
        ramBar.maxValue = deck.ram;
        ramBar.value = deck.ram;
    }

    // Update is called once per frame
    void Update()
    {
        ramBar.value = deck.ram;
    }
}
