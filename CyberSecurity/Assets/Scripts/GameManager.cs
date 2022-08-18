using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public WinLose lose;
    public WinLose win;
    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            lose.Setup();
        }
        
    }

    public void WinGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            win.Setup();
        }
    }

}
