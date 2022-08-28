using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Cleanse", menuName = "Effect/Cleanse")]
public class Cleanse : Effect
{
    UnitManager manager;
    public GameObject cleanseExpansion;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> cleanseTiles = manager.selectedCharacter.Select(true);
        manager.grid.HighlightGrid(cleanseTiles);
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

                    if (cleanseTiles.Contains(node) && character != null)
                    {

                        if (character.CompareTag("Security Control") &&
                            character.GetComponent<Unit>().health > 0 &&
                            character.GetComponent<Unit>().isCorrupted != 0 &&
                            character.GetComponent<Unit>().isStunned)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Cleanse");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            if (SceneManager.GetActiveScene().name == "Level2")
                            {
                                manager.objectives.GetComponent<Level2Object>().cleanse.SetActive(false);
                                manager.objectives.GetComponent<Level2Object>().cleansecomp.SetActive(true);
                            }

                            if(SceneManager.GetActiveScene().name == "Level3")
                            {
                                manager.GetComponent<Level3Object>().Cleansing();
                            }

                            Instantiate(cleanseExpansion, character.transform);
                            character.GetComponent<Unit>().isCorrupted = 0;
                            character.GetComponent<Unit>().isStunned = false;
                            if (manager.selectedCharacter.isBuffed)
                            {
                                character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.1f);
                                manager.selectedCharacter.isBuffed = false;
                                Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
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

