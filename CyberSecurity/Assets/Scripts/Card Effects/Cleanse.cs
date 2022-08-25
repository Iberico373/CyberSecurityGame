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
                            character.GetComponent<Unit>().health > 0 &&
                            character.GetComponent<Unit>().isCorrupted != 0)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Heal");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            if (SceneManager.GetActiveScene().name == "Level1")
                            {
                                manager.objectives.GetComponent<Level1Object>().heal.SetActive(false);
                                manager.objectives.GetComponent<Level1Object>().healcomp.SetActive(true);
                            }
                            Instantiate(cleanseExpansion, character.transform);
                            character.GetComponent<Unit>().isCorrupted = 0;
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

