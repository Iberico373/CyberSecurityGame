using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "DSAction", menuName = "Effect/DSAction")]
public class DSAction : Effect
{
    UnitManager manager;
    CardTutorial tutorial;
    int dsActionRadius = 2;

    public GameObject effect;
    public override void UseEffect()
    {
        manager = UnitManager.instance;

        if (GameObject.Find("Tutorial Canvas 1") != null)
        {
            tutorial = GameObject.Find("Tutorial Canvas 1").GetComponent<CardTutorial>();
        }

        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> actionTiles = manager.selectedCharacter.Select(true, dsActionRadius);
        manager.grid.HighlightGrid(actionTiles);

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Node node = manager.grid.NodeFromWorldPoint(hit.collider.transform.position);
                GameObject character = node.ReturnObject();


                if (manager.selectedCharacter != null)
                {
                    if (actionTiles.Contains(node) && character != null)
                    {
                        if (character.GetComponent<Unit>().id == 1)
                        {
                            Instantiate(effect, character.transform);
                            if (manager.selectedCharacter.name.Equals("Preventative Control"))
                            {
                                character.GetComponent<DataStructure>()._currentState = State.Preventative;
                                manager.selectedCharacter.GetComponent<Unit>().UseCard();
                                
                                if (SceneManager.GetActiveScene().name == "TestLevel")
                                {
                                    tutorial.SetTutorial(2);
                                    manager.objectives.GetComponent<TutorialObject>().takeover.SetActive(false);
                                    manager.objectives.GetComponent<TutorialObject>().takeovercomp.SetActive(true);
                                    manager.objective1 = true;
                                }
                                
                            }

                            else if (manager.selectedCharacter.name.Equals("Detective Control"))
                            {
                                character.GetComponent<DataStructure>()._currentState = State.Detective;
                                manager.selectedCharacter.GetComponent<Unit>().UseCard();
                                
                                if (SceneManager.GetActiveScene().name == "TestLevel")
                                {
                                    manager.objectives.GetComponent<TutorialObject>().takeover.SetActive(false);
                                    manager.objectives.GetComponent<TutorialObject>().takeovercomp.SetActive(true);
                                    tutorial.SetTutorial(2);
                                    manager.objective1 = true;
                                }
                            }

                            else if (manager.selectedCharacter.name.Equals("Deterrent Control"))
                            {
                                character.GetComponent<DataStructure>()._currentState = State.Deterrent;
                                manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            }

                            else if (manager.selectedCharacter.name.Equals("Corrective Control"))
                            {
                                character.GetComponent<DataStructure>().isLocked = false;
                                character.GetComponent<DataStructure>()._currentState = State.None;
                                manager.selectedCharacter.GetComponent<Unit>().UseCard();

                                if (SceneManager.GetActiveScene().name == "Level2")
                                {
                                    manager.objectives.GetComponent<Level2Object>().cleanse.SetActive(false);
                                    manager.objectives.GetComponent<Level2Object>().cleansecomp.SetActive(true);
                                    manager.objective2 = true;
                                }

                                if (SceneManager.GetActiveScene().name == "Level3")
                                {
                                    manager.GetComponent<Level3Object>().Cleansing();
                                }
                            }

                            manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " captured a ", character.name);
                        }                                   
                    }

                    manager.selectedCharacter.GetComponent<Unit>().DeselectCard();

                    manager.grid.ClearGrid();
                    manager.effect = null;
                }
            }
        }
    }
}
