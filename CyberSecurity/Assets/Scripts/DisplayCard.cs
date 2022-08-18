using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    public CardSO cardSO;
    public int id;
    public string cardName;
    public int cost;
    public string cardDesc;
    public Image cardArt;
    public GameObject cardHighlight;

    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI costText;
    public TMPro.TextMeshProUGUI descText;

    Button button;
    bool dragging;
    int oldIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void GetCardByID(int id)
    {
        SetUI(Database.GetCardId(id));
    }

    public void GetRandomCard()
    {
        SetUI(Database.GetRandomCard());
    }

    public void HighlightCard()
    {
        UnitManager.instance.selectedCard = transform.gameObject;
        cardHighlight.SetActive(true);
    }

    public void OnPointerDown()
    {
        GameObject.Find("Camera Pivot").GetComponent<CameraMove>().playing = true;
        dragging = true;
        oldIndex = transform.GetSiblingIndex();
        transform.SetParent(UnitManager.instance.selectedCharacter.GetComponent<Deck>().deckParent);            
    }

    public void OnPointerUp()
    {
        dragging = false;
        GameObject.Find("Camera Pivot").GetComponent<CameraMove>().playing = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name != "Deck Panel")
            {
                
                if (UnitManager.instance.selectedCard != null)
                {
                    UnitManager.instance.selectedCharacter.GetComponent<Unit>().DeselectCard();
                }

                if (UnitManager.instance.selectedCharacter.GetComponent<Deck>().ram > 0)
                {
                    GameObject.Find("Camera Pivot").GetComponent<CameraMove>().selecting = true;
                    HighlightCard();
                    cardSO.cardEffect.UseEffect();
                }
            }
        }
        
        transform.SetParent(UnitManager.instance.selectedCharacter.GetComponent<Deck>().panel);
        transform.SetSiblingIndex(oldIndex);
    }

    private void SetUI(CardSO card)
    {
        cardSO = card;
        id = card.id;
        cardName = card.cardName;
        cost = card.cost;
        cardDesc = card.cardDesc;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descText.text = cardDesc;
        cardArt.sprite = card.cardArtwork;
    }
}
