using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Blast", menuName = "Effect/Blast")]
public class Blast : Effect
{
    UnitManager manager;
    public GameObject target;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        List<Node> blastTiles = manager.grid.GetNeighbours(manager.grid.NodeFromWorldPoint(manager.selectedCharacter.transform.position), 2);

        foreach (Node n in blastTiles)
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

                    if (blastTiles.Contains(node) && character.CompareTag("Malware"))
                    {
                        manager.selectedCharacter.transform.LookAt(character.transform.position);
                        manager.selectedCharacter.anim.SetTrigger("Attack");
                        manager.selectedCharacter.GetComponent<Unit>().UseCard();
                        if (SceneManager.GetActiveScene().name == "Level1")
                        {
                            manager.objectives.GetComponent<Level1Object>().push.SetActive(false);
                            manager.objectives.GetComponent<Level1Object>().pushcomp.SetActive(true);
                        }
                        target = character;
                        //Vector3 dir = (character.transform.position - manager.selectedCharacter.transform.position).normalized;
                        //character.transform.position = character.transform.position + dir * manager.grid.nodeRadius * 2;
                        //character.GetComponent<BaseAI>().aggrolist.Insert(0, manager.selectedCharacter.gameObject);
                        //manager.grid.UpdateGrid();
                    }

                    manager.selectedCharacter.GetComponent<Unit>().DeselectCard();

                    manager.grid.ClearGrid();
                    manager.effect = null;
                }
            }
        }
    }
}
