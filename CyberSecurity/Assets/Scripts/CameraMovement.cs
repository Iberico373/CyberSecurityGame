using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Vector3 resetPos;
    [SerializeField]
    private int speed;

    [SerializeField]
    private int mapMinX;
    [SerializeField]
    private int mapMaxX;
    [SerializeField]
    private int mapMinZ;
    [SerializeField]
    private int mapMaxZ;

    public bool playing;

    private Vector3 dragOrigin;
    private Vector3 dragCurrentPos;
    private Vector3 newPos;

    private void Start()
    {
        resetPos = transform.position;
        newPos = resetPos;
    }

    private void Update()
    {
        if (!playing)
        {
            PanCamera();
            transform.position = newPos;
        }        
    }

    void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragOrigin = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);

                newPos = ClampCamera(transform.position + (dragOrigin - dragCurrentPos));
            }
        }
    }

    Vector3 ClampCamera(Vector3 targetPosition)
    {
        if(targetPosition.x < mapMinX || targetPosition.x > mapMaxX || targetPosition.z > mapMaxZ || targetPosition.z < mapMinZ)
        {
            return transform.position;
        }

        else
        {
            return targetPosition;
        }
    }
}
