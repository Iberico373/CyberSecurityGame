using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Heal", menuName = "Effect/Heal")]
public class Heal : Effect
{
    public int heal;
    public GameObject healExpansion;

    UnitManager manager;

    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;
        manager.pathfinding.radius = 2;
        HashSet<Node> scanTiles = manager.selectedCharacter.Select(true);
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
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Heal");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();

                            if (SceneManager.GetActiveScene().name == "Level1")
                            {
                                manager.objectives.GetComponent<Level1Object>().heal.SetActive(false);
                                manager.objectives.GetComponent<Level1Object>().healcomp.SetActive(true);
                            }

                            if(SceneManager.GetActiveScene().name == "Level3")
                            {
                                manager.GetComponent<Level3Object>().Healing();
                            }

                            Instantiate(healExpansion, character.transform);

                            if (character.GetComponent<Unit>().health + Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.2f) > character.GetComponent<Unit>().maxHealth)
                            {
                                character.GetComponent<Unit>().health = character.GetComponent<Unit>().maxHealth;
                            }

                            else
                            {
                                if(manager.selectedCharacter.isBuffed)
                                {
                                    if(character.GetComponent<Unit>().health + Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.4f) > character.GetComponent<Unit>().maxHealth)
                                    {
                                        character.GetComponent<Unit>().health = character.GetComponent<Unit>().maxHealth;
                                        manager.selectedCharacter.isBuffed = false;
                                        Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                                    }
                                    else
                                    {
                                        character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.4f);
                                        manager.selectedCharacter.isBuffed = false;
                                        Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                                    }
                                }
                                else
                                {
                                    character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.2f);
                                }                            }
                           
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
