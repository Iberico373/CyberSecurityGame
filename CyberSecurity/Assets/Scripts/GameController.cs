using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool gameEnded = false;

    public GameObject Lose;

    public GameObject Win;

    public void GameOver()
    {
        gameEnded = true;
        Lose.SetActive(true);
    }

    public void GameWon()
    {
        gameEnded = true;
        Win.SetActive(true);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        //if (ObjectiveStats.Hp <= 0)
        //{
        //    GameOver();
        //}

        //if (ObjectiveStats.Hp >= 0 && allenemydied)
        //{
        //    GameWon();
        //}
    }
}
