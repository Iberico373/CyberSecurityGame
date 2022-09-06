using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Cleanse", menuName = "Effect/Cleanse")]
public class Cleanse : Effect
{
    UnitManager manager;
    int cleanseRadius = 2;

    public GameObject cleanseExpansion;

    public override void UseEffect()
    {
        manager = UnitManager.instance;
        manager.grid.ClearGrid();
        manager.effect = this;

        HashSet<Node> cleanseTiles = manager.selectedCharacter.Select(true, cleanseRadius);
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
                            character.GetComponent<Unit>().corrupt > 0 ||
                            character.GetComponent<Unit>().stun > 0)
                        {
                            manager.selectedCharacter.transform.LookAt(character.transform);
                            manager.selectedCharacter.anim.SetTrigger("Cleanse");
                            manager.selectedCharacter.GetComponent<Unit>().UseCard();

                            for (int i = 0; i < character.GetComponent<Unit>().corrupt; i++)
                            {
                                Destroy(character.transform.Find("CorruptionEffect(Clone)").gameObject);
                            }
                            Instantiate(cleanseExpansion, character.transform);

                            character.GetComponent<Unit>().corrupt = 0;
                            character.GetComponent<Unit>().stun = 0;

                            if (manager.selectedCharacter.buffed > 0)
                            {
                                character.GetComponent<Unit>().health += Mathf.RoundToInt(character.GetComponent<Unit>().maxHealth * 0.15f);
                                manager.selectedCharacter.buffed = 0;
                                Destroy(manager.selectedCharacter.transform.Find("BuffAura(Clone)").gameObject);
                            }

                            manager.battleLog.UpdateBattleLog(manager.selectedCharacter.name, " cleansed ", character.name + " of all debuffs");
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

