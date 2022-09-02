using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public GameObject scan;
    public GameObject scancomp;
    public GameObject takeover;
    public GameObject takeovercomp;
    public GameObject kill;
    public GameObject killcomp;

    // Start is called before the first frame update
    void Start()
    {
        scan = transform.Find("Scan").gameObject;
        scancomp = transform.Find("Scancomp").gameObject;
        takeover = transform.Find("Takeover").gameObject;
        takeovercomp = transform.Find("Takeovercomp").gameObject;
        kill = transform.Find("Kill").gameObject;
        killcomp = transform.Find("Killcomp").gameObject;

        scancomp.SetActive(false);
        takeovercomp.SetActive(false);
        killcomp.SetActive(false);
    }
}
