using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu (fileName = "Walk", menuName = "Effect/Walk")]
public class Walk : Effect
{
    UnitManager manager;
    CardTutorial tutorial;
    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;
        
        HashSet<Node> movementTiles = manager.selectedCharacter.Select(false, manager.selectedCharacter.movementSpeed);
        manager.grid.HighlightGrid(movementTiles);

        if (GameObject.Find("Tutorial Canvas 1") != null)
        {
            tutorial = GameObject.Find("Tutorial Canvas 1").GetComponent<CardTutorial>();
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

                    if (movementTiles.Contains(node) && character == null)
                    {
                        manager.selectedCharacter.GetComponent<Unit>().UseCard();
                        if (SceneManager.GetActiveScene().name == "TestLevel")
                        {
                            tutorial.SetTutorial(1);
                        }

                        manager.selectedCharacter.Move(node.worldPos);
                    }

                    manager.selectedCharacter.GetComponent<Unit>().DeselectCard();

                    manager.grid.ClearGrid();
                    manager.effect = null;
                }
            }
        }
    }
}
