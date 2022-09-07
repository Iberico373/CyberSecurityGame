using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    public GameObject battleLog;
    public TMPro.TextMeshProUGUI battleText;

    bool isActive;
    float delay;
    public float time;
    bool countdown;
    private void Start()
    {
        isActive = false;
        delay = time;
    }

    public void UpdateBattleLog(string characterName, string action, string targetName = "")
    {
       if (!isActive)
        {
            battleLog.SetActive(true);
            isActive = true;
        }
        delay = time;
        battleText.text = characterName + action + targetName;
    }

    private void Update()
    {
        if(isActive)
        {
            delay -= 1;
            countdown = true;
        }
        if(delay <= 0 && countdown)
        {
            isActive = false;
            battleLog.SetActive(false);
            countdown = false;
        }
    }

    IEnumerator DelayedDeactivate()
    {
        yield return new WaitForSeconds(2);
        battleLog.SetActive(false);
        isActive = false;
    }
}
