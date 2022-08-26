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
    public Sprite deterrent;
    public Sprite corrective;
    public Sprite compensating;
    public Sprite dWalk;
    public Sprite pWalk;
    public Sprite rWalk;
    public Sprite dtWalk;
    public Sprite cWalk;
    public Sprite comWalk;
    public Sprite dDS;
    public Sprite pDS;
    public Sprite rDS;
    public Sprite dtDS;
    public Sprite cDS;
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
        else if (manager.selectedCharacter.name == "Corrective Control")
        {
            img.sprite = corrective;
            if (card.id == 1)
            {
                card.cardArt.sprite = cWalk;
            }
            if (card.id == 4)
            {
                card.cardArt.sprite = cDS;
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

        else if (manager.selectedCharacter.name == "Deterrent Control")
        {
            img.sprite = deterrent;

            if (card.id == 1)
            {
                card.cardArt.sprite = dtWalk;
            }

            else if (card.id == 4)
            {
                card.cardArt.sprite = dtDS;
            }
        }
        else if (manager.selectedCharacter.name == "Compensating Control")
        {
            img.sprite = compensating;

            if (card.id == 1)
            {
                card.cardArt.sprite = comWalk;
            }

            //else if (card.id == 4)
            //{
            //    card.cardArt.sprite = dtDS;
            //}
        }
    }
}
