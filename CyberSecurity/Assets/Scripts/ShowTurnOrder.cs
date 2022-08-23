using UnityEngine;
using UnityEngine.UI;


public class ShowTurnOrder : MonoBehaviour
{
    public GameObject turnList;
    public Vector3 hidePos;
    public Vector3 showPos;

    public bool hide = true;

    public void Show()
    {
        if (hide)
        {
            turnList.transform.GetComponent<RectTransform>().localPosition = showPos;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            hide = false;
        }
    }

    public void Hide()
    {
        if (!hide)
        {
            turnList.transform.GetComponent<RectTransform>().localPosition = hidePos;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            hide = true;
        }
    }
}
