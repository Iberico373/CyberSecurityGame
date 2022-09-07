using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHighlight : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scCharacterText;
    public TMPro.TextMeshProUGUI mCharacterText;
    public TMPro.TextMeshProUGUI scHealthText;
    public TMPro.TextMeshProUGUI mHealthText;
    public GameObject scHealth; 
    public GameObject mHealth;
    public GameObject scTargetHighlightBase;
    public GameObject mTargetHighlightBase;

    private void Update()
    {
        HighlightTarget();
    }

    void HighlightTarget()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            mTargetHighlightBase.SetActive(false);
            scTargetHighlightBase.SetActive(false);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Security Control") || hit.collider.CompareTag("Objective"))
                {
                    mTargetHighlightBase.SetActive(false);
                    scTargetHighlightBase.SetActive(true);

                    UpdateTargetValues(hit.collider.gameObject, scHealth.GetComponent<Slider>(), scHealthText, scCharacterText);
                }

                else if (hit.collider.CompareTag("Malware"))
                {
                    mTargetHighlightBase.SetActive(true);
                    scTargetHighlightBase.SetActive(false);

                    UpdateTargetValues(hit.collider.gameObject, mHealth.GetComponent<Slider>(), mHealthText, mCharacterText);
                }
            }
        }
    }

    void UpdateTargetValues(GameObject targetedCharacter, Slider slider, TMPro.TextMeshProUGUI sliderText, TMPro.TextMeshProUGUI characterText)
    {
        characterText.text = targetedCharacter.name;
        sliderText.text = targetedCharacter.GetComponent<Unit>().health.ToString();
        slider.maxValue = targetedCharacter.GetComponent<Unit>().maxHealth;
        slider.value = targetedCharacter.GetComponent<Unit>().health;
    }
}
