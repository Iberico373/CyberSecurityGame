using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Effect/Attack")]
public class Attack : Effect
{
    UnitManager manager;

    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        List<Node> attackTiles = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(manager.selectedCharacter.transform.position), 1);

        foreach (Node n in attackTiles)
        {
            n.tile.GetComponent<MeshRenderer>().material = manager.grid.attackHighlight;
        }

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

                    if (attackTiles.Contains(node) && character.CompareTag("Malware"))
                    {
                        manager.selectedCharacter.transform.LookAt(character.transform);
                        manager.selectedCharacter.anim.SetTrigger("Attack");
                        manager.selectedCharacter.GetComponent<Unit>().UseCard();

                        if (character.GetComponent<Unit>().isDetected)
                        {
                            character.GetComponent<Unit>().health -= 10;
                            character.GetComponent<Unit>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);
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
