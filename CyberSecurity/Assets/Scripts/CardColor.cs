using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardColor : MonoBehaviour
{
    public Image img;
    public Sprite detective;
    public Sprite preventative;
    public Sprite recovery;
    public Sprite dWalk;
    public Sprite pWalk;
    public Sprite rWalk;
    public Sprite dDS;
    public Sprite pDS;
    public Sprite rDS;
    public DisplayCard card;
    UnitManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = UnitManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.selectedCharacter.name == "Detective Control")
        {
            img.sprite = detective;
            if(card.id == 1)
            {
                card.cardArt.sprite = dWalk;
            }
            if (card.id == 4)
            {
                card.cardArt.sprite = dDS;
            }
        }

        else if (manager.selectedCharacter.name == "Preventative Control")
        {
            img.sprite = preventative;
            if (card.id == 1)
            {
                card.cardArt.sprite = pWalk;
            }
            if (card.id == 4)
            {
                card.cardArt.sprite = pDS;
            }
        }

        else if (manager.selectedCharacter.name == "Recovery Control")
        {
            img.sprite = recovery;

            if (card.id == 1)
            {
                card.cardArt.sprite = rWalk;
            }
        }
    }
}
