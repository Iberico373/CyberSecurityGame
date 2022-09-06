using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Heal", menuName = "Effect/Heal")]
public class Heal : Effect
{
    UnitManager manager;
    int healRadius = 2;

    public int heal;
    public GameObject healExpansion;

    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> scanTiles = manager.selectedCharacter.Select(true, healRadius);
        manager.grid.HighlightGrid(scanTiles);

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

                    if (scanTiles.Contains(node) && character != null)
                    {

                        if (character.CompareTag("Security Control") && character.GetComponent<Unit>().health > 0 
                            && character.GetComponent<Unit>().health != character.GetComponent<Unit>().maxHealth)
                        {
                            if (SceneManager.GetActiveScene().name == "Level1")
                            {
                                manager.objectives.GetComponent<Level1Object>().heal.SetActive(false);
                                manager.objectives.GetComponent<Level1Object>().healcomp.SetActive(true);
                                manager.objective1 = true;
                            }


                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Heal");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            Instantiate(healExpansion, character.transform);

                            if (character.GetComponent<Unit>().health + Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.2f)
                                > character.GetComponent<Unit>().maxHealth)
                            {
                                manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " healed ",
                                        character.name + " for " + character.GetComponent<Unit>().maxHealth * character.GetComponent<Unit>().health + " health!");
                                character.GetComponent<Unit>().health = character.GetComponent<Unit>().maxHealth;                                
                            }

                            else
                            {
                                if (manager.selectedCharacter.buffed > 0)
                                {
                                    if (character.GetComponent<Unit>().health + Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.5f)
                                        > character.GetComponent<Unit>().maxHealth)
                                    {
                                        manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " healed " + character.name + " to full!");
                                        character.GetComponent<Unit>().health = character.GetComponent<Unit>().maxHealth;
                                    }

                                    else
                                    {
                                        character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.5f);
                                        manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " healed ",
                                        character.name + " for " + character.GetComponent<Unit>().maxHealth * 0.5f + " health!");
                                    }

                                    manager.selectedCharacter.buffed -= 1;
                                    Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                                }

                                else
                                {
                                    character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.3f);
                                    manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " healed ", 
                                        character.name + " for " + character.GetComponent<Unit>().maxHealth * 0.3f + " health!");
                                }
                            }
                           
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
