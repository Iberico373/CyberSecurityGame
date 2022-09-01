using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    GameObject bullet;
    GameObject objective;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        bullet = this.gameObject;
        objective = GameObject.Find("Objective");
    }

    // Update is called once per frame
    void Update()
    {
        bullet.transform.Translate((-bullet.transform.position - -objective.transform.position) * Time.deltaTime * speed,Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
