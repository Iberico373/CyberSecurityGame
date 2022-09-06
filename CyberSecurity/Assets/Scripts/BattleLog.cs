using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    public TMPro.TextMeshProUGUI battleText;

    public void UpdateBattleLog(string characterName, string action, string targetName = "")
    {
        battleText.text = characterName + action + targetName;
    }
}
