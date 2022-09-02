using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHighlight : MonoBehaviour
{
    public TMPro.TextMeshProUGUI characterText;
    public TMPro.TextMeshProUGUI scHealthText;
    public TMPro.TextMeshProUGUI mHealthText;
    public TMPro.TextMeshProUGUI attackText;
    public GameObject scHealth; 
    public GameObject mHealth;
    public GameObject targetHighlightBase;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            targetHighlightBase.SetActive(false);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Security Control") || hit.collider.CompareTag("Objective"))
                {
                    targetHighlightBase.SetActive(true);

                    scHealth.SetActive(true);
                    mHealth.SetActive(false);   

                    characterText.text = hit.collider.name;
                    scHealthText.text = hit.collider.GetComponent<Unit>().health.ToString();
                    attackText.text = "N/A";
                    scHealth.GetComponent<Slider>().maxValue = hit.collider.GetComponent<Unit>().maxHealth;
                    scHealth.GetComponent<Slider>().value = hit.collider.GetComponent<Unit>().health;
                }

                else if (hit.collider.CompareTag("Malware"))
                {
                    targetHighlightBase.SetActive(true);

                    scHealth.SetActive(false);
                    mHealth.SetActive(true);

                    characterText.text = hit.collider.name;
                    mHealthText.text = hit.collider.GetComponent<Unit>().health.ToString();
                    attackText.text = "N/A";
                    mHealth.GetComponent<Slider>().maxValue = hit.collider.GetComponent<Unit>().maxHealth;
                    mHealth.GetComponent<Slider>().value = hit.collider.GetComponent<Unit>().health;
                }
            }
        }
    }
}
