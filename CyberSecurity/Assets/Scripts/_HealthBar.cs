using UnityEngine;
using UnityEngine.UI;

public class _HealthBar : MonoBehaviour
{
    public Unit character;
    public TMPro.TextMeshProUGUI healthText;
    public int characterCurrentHP;
    int characterHP;

    private void Awake()
    {
        characterHP = character.health;
        transform.GetComponent<Slider>().maxValue = characterHP;
    }

    private void Update()
    {
        characterCurrentHP = character.health;

        transform.GetComponent<Slider>().value = characterCurrentHP;
        healthText.text = characterCurrentHP.ToString();
    }
}
