using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI totalDraw;
    public TMPro.TextMeshProUGUI totalDiscard;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI currentRam;
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

        totalDraw.text = currentDeck.deck.Count.ToString();
        totalDiscard.text = currentDeck.discard.Count.ToString();

        healthBar.value = character.health;
        healthText.text = character.health.ToString();

        ramBar.value = currentDeck.ram;
        currentRam.text = currentDeck.ram.ToString();
    }

    public void UpdateUIValues()
    {
        character = manager.selectedCharacter;
        currentDeck = character.GetComponent<Deck>();
    }
}
