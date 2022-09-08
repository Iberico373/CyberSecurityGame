using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public MyGrid grid;
    public Pathfinding pathfinding;
    public Unit selectedCharacter;
    public Effect effect;
    public DisplayTurnOrder displayTurnOrder;
    public BattleLog battleLog;
    public GameObject selectedCard;
    public GameObject unitGroup;
    public GameObject deck;
    public GameObject objectives;
    public List<Unit> unitList;

    //objective variables for triggering win
    public bool objective1;
    public bool objective2;
    public int slaughtered;
    public int murderGoals;

    DeckUI deckUI;

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

        deckUI = deck.GetComponent<DeckUI>();

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

        selectedCharacter = unitList[0];
    }

    public void AddCloneUnitToTurnOrder(GameObject clone)
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].id == clone.GetComponent<Unit>().id)
            {
                unitList.Insert(i + 1, clone.GetComponent<Unit>());
                break;
            }
        }
    }

    void NextUnit()
    {
        Unit temp = unitList[0];
        unitList.Remove(unitList[0]);
        unitList.Add(temp);
        selectedCharacter = unitList[0];
    }

    void SwitchState()
    {
        switch (selectedCharacter.tag)
        {
            case "Objective":
                deck.SetActive(false);
                Invoke("EndTurn", 2);
                break;

            case "Data Structure":
                deck.SetActive(false);
                selectedCharacter.CheckStatus();

                if (selectedCharacter.GetComponent<DataStructure>()._currentState != State.None)
                {
                    selectedCharacter.GetComponent<DataStructure>().StateEffect();
                }

                Invoke("EndTurn", 2);
                break;

            case "Security Control":
                selectedCharacter.CheckStatus();

                if (selectedCharacter.GetComponent<Unit>().isAlive)
                {
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                }

                break;

            case "Malware":
                if (selectedCharacter.GetComponent<BaseAI>() == null)
                {
                    Debug.Log(selectedCharacter.name + "does not have the BaseAI script");
                }

                else
                {
                    deck.SetActive(false);
                    selectedCharacter.CheckStatus();                    
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<BaseAI>().SelectTarget();
                }

                break;

            default:
                Invoke("EndTurn", 2);
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

        if (selectedCharacter.pointer != null)
        {
            selectedCharacter.pointer.SetActive(false);
        }        

        NextUnit();
        deckUI.UpdateUIValues();
        displayTurnOrder.UpdateTurnOrder();

        SwitchState();
    }
}
