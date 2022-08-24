using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Heal", menuName = "Effect/Heal")]
public class Heal : Effect
{
    UnitManager manager;
    public int heal;
    public GameObject healExpansion;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

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

                        if (character.CompareTag("Security Control") && 
                            character.GetComponent<Unit>().downed == false && 
                            character.GetComponent<Unit>().health != character.GetComponent<Unit>().maxHealth)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Heal");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            if (SceneManager.GetActiveScene().name == "Level1")
                            {
                                manager.objectives.GetComponent<Level1Object>().heal.SetActive(false);
                                manager.objectives.GetComponent<Level1Object>().healcomp.SetActive(true);
                            }
                            Instantiate(healExpansion, character.transform);

                            if (character.GetComponent<Unit>().health + heal > character.GetComponent<Unit>().maxHealth)
                            {
                                character.GetComponent<Unit>().health = character.GetComponent<Unit>().maxHealth;
                            }

                            else
                            {
                                character.GetComponent<Unit>().health += 20;
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
