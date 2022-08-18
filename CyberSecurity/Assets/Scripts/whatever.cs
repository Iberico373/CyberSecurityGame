using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whatever : MonoBehaviour
{
    public bool playing;
    public Camera cam;

    Vector3 dragOrigin;

    private void Update()
    {
        if (!playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += diff;
            }
        }
    }
}
