using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Update", menuName = "Effect/Update")]
public class Update : Effect
{
    UnitManager manager;
    public GameObject buffAura;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> throttleTiles = manager.selectedCharacter.Select(true);
        manager.grid.HighlightGrid(throttleTiles);
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

                    if (throttleTiles.Contains(node) && character != null)
                    {

                        if (character.CompareTag("Security Control") &&
                            character.GetComponent<Unit>().health > 0)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Action");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();
                            if (manager.selectedCharacter.isThrottled <= 4)
                            {
                                manager.selectedCharacter.isThrottled = 4;
                            }
                            Instantiate(buffAura, character.transform);
                            if (SceneManager.GetActiveScene().name == "Level2")
                            {
                                //Put whatever condition here I guess
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
