using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour
{
    public CardDatabase cards;
    public static Database instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public static CardSO GetCardId(int id)
    {
        return instance.cards.allCards.FirstOrDefault(i => i.id == id);
    }

    public static CardSO GetRandomCard()
    {
        return instance.cards.allCards[Random.Range(0, instance.cards.allCards.Count)];
    }
}
