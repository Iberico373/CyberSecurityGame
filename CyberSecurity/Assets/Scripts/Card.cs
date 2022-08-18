using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public int id;
    public string cardName;
    public Effect cardEffect;
    [TextArea]
    public string cardDesc;
    public int cost;
    public Sprite cardArtwork;
    public string cardType;
    public int maxAmount;
}
