using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public WinLose lose;
    public WinLose win;
    public AudioSource winSound;
    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            StartCoroutine("Delay");
            lose.Setup();
        }
        
    }

    public void WinGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        winSound.Play();
        yield return new WaitForSecondsRealtime(3);
        win.Setup();
    }

}
