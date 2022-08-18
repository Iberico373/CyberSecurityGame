using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Card Database", menuName ="Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardSO> allCards;
}

