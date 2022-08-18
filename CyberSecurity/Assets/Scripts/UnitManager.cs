using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public MyGrid grid;
    public Pathfinding pathfinding;
    public Unit selectedCharacter;
    public Effect effect;
    public DisplayTurnOrder displayTurnOrder;
    public GameObject selectedCard;
    public GameObject unitGroup;
    public GameObject preventativeDeck;
    public GameObject detectiveDeck;
    public GameObject recoveryDeck;
    public GameObject deterrentDeck;
    public GameObject turnManager;
    public List<Unit> unitList;
    public GameObject Takeover;
    public GameObject Takeovercomplete;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        SortTurnOrder();
        SwitchState();
    }

    private void Update()
    {
        if (effect != null)
        {
            effect.UseEffect();
        }
        
        else if (selectedCard != null)
        {
            selectedCard.GetComponent<DisplayCard>().cardHighlight.SetActive(false);
            grid.ClearGrid();
            selectedCard = null;
        }
    }

    public void SortTurnOrder()
    {
        unitList = new List<Unit>(unitGroup.GetComponentsInChildren<Unit>());
        Unit temp;

        for (int j = 0; j <= unitList.Count - 2; j++)
        {
            for (int i = 0; i <= unitList.Count - 2; i++)
            {
                if (unitList[i].speed < unitList[i + 1].speed)
                {
                    temp = unitList[i + 1];
                    unitList[i + 1] = unitList[i];
                    unitList[i] = temp;
                }
            }
        }
    }

    void NextUnit()
    {
        Unit temp = unitList[0];
        unitList.Remove(unitList[0]);
        unitList.Insert(unitList.Count, temp);
    }

    void SwitchState()
    {
        selectedCharacter = unitList[0];

        switch (selectedCharacter.id)
        {
            case 0:
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(true);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(false);
                    deterrentDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case 1:
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(true);
                    recoveryDeck.SetActive(false);
                    deterrentDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case 2:
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(true);
                    deterrentDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case 3:
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(false);
                    deterrentDeck.SetActive(true);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case 4:
                if (selectedCharacter == null)
                {
                    EndTurn();
                }

                else
                {
                    if (selectedCharacter.isStunned)
                    {
                        selectedCharacter.CheckStatus();
                        EndTurn();
                        break;
                    }

                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(false);
                    deterrentDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<RansomwareAI>().SelectTarget();
                }

                break;

            case 5:
                if (selectedCharacter == null)
                {
                    EndTurn();
                }

                else
                {
                    if (selectedCharacter.isStunned)
                    {
                        selectedCharacter.CheckStatus();
                        EndTurn();
                        break;
                    }

                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<VirusAI>().SelectTarget();
                }
                
                break;

            case 6:
                if (selectedCharacter == null)
                {
                    EndTurn();
                }

                else
                {
                    if (selectedCharacter.isStunned)
                    {
                        selectedCharacter.CheckStatus();
                        EndTurn();
                        break;
                    }

                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(false);
                    deterrentDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<BotAI>().SelectTarget();
                }

                break;

            case 7:
                preventativeDeck.SetActive(false);
                detectiveDeck.SetActive(false);
                selectedCharacter.pointer.SetActive(true);
                selectedCharacter.CheckStatus();
                if (selectedCharacter.GetComponent<DataStructure>()._currentState != State.None)
                {
                    selectedCharacter.GetComponent<DataStructure>().StateEffect();
                }

                EndTurn();
                break;

            default:
                EndTurn();
                break;
        }
    }

    public void EndTurn()
    {
        if (selectedCharacter.CompareTag("Security Control"))
        {
            effect = null;
            selectedCharacter.GetComponent<Deck>().Discard();
        }
        selectedCharacter.pointer.SetActive(false);
        NextUnit();
        displayTurnOrder.UpdateTurnOrder();
        SwitchState();
    }
}
