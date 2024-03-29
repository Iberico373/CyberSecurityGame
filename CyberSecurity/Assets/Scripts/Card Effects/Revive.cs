using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Revive", menuName = "Effect/Revive")]
public class Revive : Effect
{
    UnitManager manager;
    int reviveRadius = 1;

    public GameObject healExpansion;

    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> scanTiles = manager.selectedCharacter.Select(true, reviveRadius);
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

                            character.GetComponent<Unit>().health = Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.5f);
                            character.GetComponent<Unit>().isAlive = true;
                            manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " revived ", character.name);
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