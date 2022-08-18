using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    //Note,uncomment the transform.lookat and gameobject cam then comment the start function and transform.rotation = rot when trying to find good angle for ui element attached to object
    //do the reverse of the above when done and have set the optimal view angle
    Quaternion rot;
    //public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rot;
        //transform.LookAt(cam.transform);
    }
}
