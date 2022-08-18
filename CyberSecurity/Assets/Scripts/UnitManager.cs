using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    void SortTurnOrder()
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

        switch (selectedCharacter.name)
        {
            case "Preventative Control":
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(true);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case "Detective Control":
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(true);
                    recoveryDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            case "Recovery Control":
                if (selectedCharacter.downed)
                {
                    EndTurn();
                }

                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    recoveryDeck.SetActive(true);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;
            case "Bot":
                if (selectedCharacter == null)
                {
                    EndTurn();
                }
                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<RansomwareAI>().SelectTarget();
                }
                
                break;
            case "Virus":
                if (selectedCharacter == null)
                {
                    EndTurn();
                }
                else
                {
                    preventativeDeck.SetActive(false);
                    detectiveDeck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<VirusAI>().SelectTarget();
                }
                
                break;
            case "Data Structure":
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
