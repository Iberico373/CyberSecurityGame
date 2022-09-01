using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI ramText;
    public Slider healthBar;
    public Slider ramBar;

    UnitManager manager;
    public Unit character;
    public Deck currentDeck;

    private void Awake()
    {
        manager = UnitManager.instance;
        character = manager.selectedCharacter;
        currentDeck = character.GetComponent<Deck>();
    }

    private void Update()
    {
        if (!character.CompareTag("Security Control"))
        {
            return;
        }

        nameText.text = character.name;

        healthBar.maxValue = character.maxHealth;
        ramBar.maxValue = 3;

        healthBar.value = character.health;
        healthText.text = character.health.ToString();

        ramBar.value = currentDeck.ram;
        ramText.text = currentDeck.ram.ToString();
    }

    public void UpdateUIValues()
    {
        character = manager.selectedCharacter;
        currentDeck = character.GetComponent<Deck>();
    }
}
