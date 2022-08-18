using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private bool isHolding = false;
    private Vector2 startPosition;
    public Transform cards;
    private Vector2 startScale;
    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //change this to phone/android
        }

        if (isHolding)
        {
            transform.localScale = new Vector2(2f, 11f);
        }

        
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
    }
    
    public void StartHold()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        isHolding = true;
    }

    public void EndHold()
    {
        isHolding = false;
        transform.localScale = startScale;

    }
}
