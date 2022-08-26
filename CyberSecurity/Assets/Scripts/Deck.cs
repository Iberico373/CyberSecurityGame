using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public List<CardSO> deck = new List<CardSO>();
    public List<CardSO> hand = new List<CardSO>();
    public List<CardSO> discard = new List<CardSO>();
    public GameObject card;
    public Transform deckParent;
    public Transform panel;
    public int deckSize;
    public int ram;

    private void Awake()
    {
        CreateDeck();
    }

    void CreateDeck()
    {
        while (deck.Count < deckSize)
        {
            for (int i = 1; i < Database.instance.cards.allCards.Count + 1; i++)
            {
                CardSO card = Database.GetCardId(i);

                if (card.cardType.Equals(gameObject.name) || card.cardType.Equals(""))
                {
                    if (transform.name.Equals("Recovery Control") || transform.name.Equals("Compensating Control"))
                    {
                        if (card == Database.GetCardId(4))
                        {
                            continue;
                        }                    
                    }

                    for (int a = 0; a < card.maxAmount; a++)
                    {
                        deck.Add(card);
                    }
                }
            }

            while (true)
            {
                CardSO randomCard = Database.GetRandomCard();

                if (randomCard.cardType.Equals(gameObject.name) || randomCard.cardType.Equals(""))
                {
                    if (transform.name.Equals("Recovery Control") || transform.name.Equals("Compensating Control"))
                    {
                        if (randomCard == Database.GetCardId(4))
                        {
                            continue;
                        }
                    }

                    deck.Add(randomCard);
                    break;
                }
            }
        }
        
    }

    public void Draw()
    {
        if (deck.Count <= 0)
        {
            Shuffle(discard);
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            hand.Add(deck[0]);
            GameObject currentCard = Instantiate(card, panel);
            currentCard.GetComponentInChildren<DisplayCard>().GetCardByID(deck[0].id);
            deck.Remove(deck[0]);
        }

        ram = 3;
    }

    public void Shuffle(List<CardSO> cardList)
    {
        int listAmount = cardList.Count;

        if (cardList == discard)
        {
            for (int i = 0; i < listAmount; i++)
            {
                deck.Add(discard[0]);
                discard.Remove(discard[0]);
            }

            Draw();
        }
    }

    public void Discard()
    {
        int handSize = hand.Count;

        for (int i = 0; i < handSize; i++)
        {
            discard.Add(hand[0]);
            hand.Remove(hand[0]);
        }

        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }
    }

    public void DiscardSingle(GameObject card)
    {
        discard.Add(card.GetComponent<DisplayCard>().cardSO);
        hand.Remove(card.GetComponent<DisplayCard>().cardSO);
        Destroy(card);
    }
}
