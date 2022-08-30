using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Revive", menuName = "Effect/Revive")]
public class Revive : Effect
{
    UnitManager manager;
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

                        if (character.CompareTag("Security Control") && character.GetComponent<Unit>().health <= 0)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Heal");
                            character.GetComponent<Unit>().anim.SetTrigger("Revive");
                            Instantiate(healExpansion, character.transform);
                            character.GetComponent<Unit>().isCorrupted = 0;
                            character.GetComponent<Unit>().health = Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.5f);
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
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