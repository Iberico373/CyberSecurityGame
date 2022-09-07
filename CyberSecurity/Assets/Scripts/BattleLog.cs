using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    public GameObject battleLog;
    public TMPro.TextMeshProUGUI battleText;

    bool isActive;

    private void Start()
    {
        isActive = false;
    }

    public void UpdateBattleLog(string characterName, string action, string targetName = "")
    {
       if (!isActive)
        {
            battleLog.SetActive(true);
            isActive = true;
        }

        battleText.text = characterName + action + targetName;
    }

    private void Update()
    {
        if(isActive)
        {
            StartCoroutine("DelayedDeactivate");
        }
    }

    IEnumerator DelayedDeactivate()
    {
        yield return new WaitForSecondsRealtime(6);
        battleLog.SetActive(false);
    }
}
