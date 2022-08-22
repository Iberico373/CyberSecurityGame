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

                newPos = transform.position + (dragOrigin - dragCurrentPos).normalized * speed * Time.deltaTime;
                //ClampCamera(transform.position + (dragOrigin - dragCurrentPos).normalized * speed * Time.deltaTime);
            }
        }
    }

    Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minZ = mapMinZ + camHeight;
        float maxZ = mapMaxZ - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newZ = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        return new Vector3(newX, targetPosition.y, newZ);
    }
}
