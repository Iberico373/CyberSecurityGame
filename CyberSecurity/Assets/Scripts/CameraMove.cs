using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject drag;
    public int xMax;
    public int xMin;
    public int zMax;
    public int zMin;
    public bool playing;
    public bool selecting;
    public int speed;
    RaycastHit hit;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
                if(selecting)
                {
                    drag.SetActive(false);
                }
                if(!selecting)
                {
                    drag.SetActive(true);
                }
                if (Input.GetMouseButton(0) && hit.collider.name == "DragArea" && xMin < transform.position.x && transform.position.x < xMax && zMin < transform.position.z && transform.position.z < zMax && !playing)
                {
                    //Debug.Log(Input.mousePosition);
                    Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
                    Vector3 pos = transform.position;
                    if (NewPosition.x > 0.0f)
                    {
                        pos += transform.right * speed * Time.deltaTime;
                    }
                    else if (NewPosition.x < 0.0f)
                    {
                        pos -= transform.right * speed * Time.deltaTime;
                    }
                    if (NewPosition.z > 0.0f)
                    {
                        pos += transform.forward * speed * Time.deltaTime;
                    }
                    if (NewPosition.z < 0.0f)
                    {
                        pos -= transform.forward * speed * Time.deltaTime;
                    }

                    pos.y = transform.position.y;
                    transform.position = pos;
                }
                if (xMin >= transform.position.x)
                {
                    transform.position -= transform.forward * speed * Time.deltaTime;
                }
                if (xMax <= transform.position.x)
                {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                if (zMin >= transform.position.z)
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                }
                if (zMax <= transform.position.z)
                {
                    transform.position -= transform.right * speed * Time.deltaTime;
                }
        }

    }
}
