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

                //newPos = transform.position + (dragOrigin - dragCurrentPos).normalized * speed * Time.deltaTime;
                newPos = ClampCamera(transform.position + (dragOrigin - dragCurrentPos).normalized * speed * Time.deltaTime);
                //ClampCamera(transform.position);
            }
        }
    }

    Vector3 ClampCamera(Vector3 targetPosition)
    {
        if(targetPosition.x < mapMinX || targetPosition.x > mapMaxX || targetPosition.z > mapMaxZ || targetPosition.z < mapMinZ)
        {
            Vector3 extraPos;

            //float camHeight = cam.orthographicSize;
            //float camWidth = cam.orthographicSize * cam.aspect;

            //float minX = mapMinX + camWidth;
            //float maxX = mapMaxX - camWidth;
            //float minZ = mapMinZ + camHeight;
            //float maxZ = mapMaxZ - camHeight;

            float newX = -(transform.position.x + (dragOrigin.x - dragCurrentPos.x) * speed * Time.deltaTime);
            float newZ = -(transform.position.z + (dragOrigin.z - dragCurrentPos.z) * speed * Time.deltaTime);
            extraPos = new Vector3(newX,0,newZ).normalized * 0.1f;
            Vector3 clampedPos = transform.position + extraPos;
            return clampedPos;

        }
        else
        {
            return targetPosition;
        }
    }
}
