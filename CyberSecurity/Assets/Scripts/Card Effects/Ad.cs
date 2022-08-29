using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ad", menuName = "Effect/Ad")]
public class Ad : Effect
{
    UnitManager manager;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        manager.selectedCharacter.GetComponent<Deck>().hand.Remove(manager.selectedCard.GetComponent<DisplayCard>().cardSO);
        Destroy(manager.selectedCard);
    }
}
