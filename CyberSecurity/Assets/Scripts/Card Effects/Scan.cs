using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scan", menuName = "Effect/Scan")]
public class Scan : Effect
{
    UnitManager manager;
    public GameObject scanExpansion;

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
                        if (character.CompareTag("Malware"))
                        {
                            if (manager.selectedCharacter.buffed > 0)
                            {
                                Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                                manager.selectedCharacter.buffed -= 1;
                            }

                            else if (manager.selectedCharacter.buffed == 0)
                            {
                                if (character.name.Equals("Trojan"))
                                {
                                    manager.selectedCharacter.GetComponent<Unit>().DeselectCard();

                                    manager.grid.ClearGrid();
                                    manager.effect = null;
                                    return;
                                }

                                manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            }

                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Scan");
                            Instantiate(scanExpansion, character.transform);

                            character.GetComponent<Unit>().isDetected = true;
                            character.GetComponent<BaseAI>().aggrolist.Remove(manager.selectedCharacter.gameObject);
                            character.GetComponent<BaseAI>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);                                                     
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

