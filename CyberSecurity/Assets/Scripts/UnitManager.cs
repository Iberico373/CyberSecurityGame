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
    public GameObject selectedCard;
    public GameObject unitGroup;
    public GameObject deck;
    public GameObject objectives;
    public List<Unit> unitList;    

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

    void NextUnit()
    {
        Unit temp = unitList[0];
        unitList.Remove(unitList[0]);
        unitList.Insert(unitList.Count, temp);
        selectedCharacter = unitList[0];
    }

    void SwitchState()
    {
        switch (selectedCharacter.id)
        {
            //Preventative's Turn
            case 0:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            //Detective's Turn
            case 1:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            //Recovery's Turn
            case 2:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            //Deterrent's Turn
            case 3:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            //Ransomware's turn
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

                    deck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<BaseAI>().SelectTarget();
                }

                break;

            //Virus' turn
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

                    deck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<VirusAI>().SelectTarget();
                }
                
                break;

            //Bot's turn
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

                    deck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.GetComponent<BotAI>().SelectTarget();
                }

                break;

            //Data Structure's turn
            case 7:
                deck.SetActive(false);
                selectedCharacter.CheckStatus();

                if (selectedCharacter.GetComponent<DataStructure>()._currentState != State.None)
                {
                    selectedCharacter.GetComponent<DataStructure>().StateEffect();
                }

                EndTurn();
                break;

            //Corrective's Turn
            case 8:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }

                break;

            //Worm's turn 
            case 9:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(false);
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                    selectedCharacter.GetComponent<WormAI>().SelectTarget();
                }

                break;

            //Objective's turn
            case 10:
                if (selectedCharacter.health <= 0)
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
                    deck.SetActive(true);
                    selectedCharacter.GetComponent<Deck>().Draw();
                    selectedCharacter.pointer.SetActive(true);
                    selectedCharacter.CheckStatus();
                }
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
