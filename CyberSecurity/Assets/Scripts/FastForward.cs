using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite fastSprite;

    Image buttonImage;
    bool normalTime = true;

    private void Start()
    {
        buttonImage = transform.GetComponent<Image>();
    }

    public void SetTime()
    {
        if (normalTime)
        {
            SetTime2X();
        }

        else
        {
            SetTime1X();
        }
    }

    void SetTime1X()
    {
        normalTime = true;

        buttonImage.sprite = normalSprite;
        Time.timeScale = 1;  
    }

    void SetTime2X()
    {
        normalTime = false;

        buttonImage.sprite = fastSprite;
        Time.timeScale = 2;
    }
}
