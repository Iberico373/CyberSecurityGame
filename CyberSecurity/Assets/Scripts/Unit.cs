using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int id;
    public int speed;
    public int health;
    public int maxHealth;
    public int movementSpeed;
    public bool isDetected;
    public bool startTurn;
    public bool downed;
    public bool isAlive;
    public bool isStunned;
    public int isCorrupted;
    public Animator anim;
    public GameObject pointer;
    public GameObject aura;
    public GameObject turnOrderDisplay;
    public List<Node> _attackTiles;
    public GameManager gameManager;
    public GameObject scanobj;
    public GameObject scancomplete;
    public GameObject Takeover;
    public GameObject Takeovercomplete;
<<<<<<< Updated upstream
    public GameObject Killbot;
    public GameObject Killbotcomplete;
=======
    //public GameObject Killbot;
    //public GameObject Killbotcomplete;
>>>>>>> Stashed changes

    PathRequestManager request;    
    Vector3[] path;
    int targetIndex;

    private void Awake()
    {
        request = GameObject.Find("Grid").GetComponent<PathRequestManager>();
        anim = GetComponent<Animator>();
        maxHealth = health;
        if (!gameObject.CompareTag("Malware"))
        {
            isDetected = true;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            downed = true;

            if (gameObject.CompareTag("Objective"))
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }
        
        if (GameObject.FindWithTag("Malware"))
        {
            isAlive = true;
            return;
        }
        
        else
        {
<<<<<<< Updated upstream
            Killbot.SetActive(false);
            Killbotcomplete.SetActive(true);
=======
            //Killbot.SetActive(false);
            //Killbotcomplete.SetActive(true);
>>>>>>> Stashed changes
            isAlive = false;
            FindObjectOfType<GameManager>().WinGame();
            isAlive = false;
            FindObjectOfType<GameManager>().WinGame();
        }
    }

    public HashSet<Node> Select()
    {
        return request.HighlightMovement(transform.position);
    }

    public void Move(Vector3 targetPos)
    {
        PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        path = newPath;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath()
    {
        anim.SetBool("Walk", true);
        targetIndex = 0;
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;

                if (movementSpeed == 0 || movementSpeed > path.Length)
                {
                    if (targetIndex >= path.Length)
                    {
                        anim.SetBool("Walk", false);
                        startTurn = false;
                        UnitManager.instance.grid.UpdateGrid();
                        yield break;
                    }
                }

                else
                {
                    if (targetIndex >= movementSpeed)
                    {
                        anim.SetBool("Walk", false);
                        _attackTiles = UnitManager.instance.grid.
                            GetNeighbours(UnitManager.instance.grid.
                            NodeFromWorldPoint(UnitManager.instance.selectedCharacter.transform.position), 1);
                        startTurn = false;
                        UnitManager.instance.grid.UpdateGrid();
                        yield break;
                    }
                }

                currentWaypoint = path[targetIndex];
            }

            transform.LookAt(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 9 * Time.deltaTime);
            yield return null;
        }
    }

    public void UseCard()
    {
        GameObject usedCard = UnitManager.instance.selectedCard;
        Deck currentDeck = UnitManager.instance.selectedCharacter.GetComponent<Deck>();

        currentDeck.ram -= usedCard.GetComponent<DisplayCard>().cost;
        currentDeck.DiscardSingle(usedCard);
    }

    public void DeselectCard()
    {
        UnitManager.instance.selectedCard.GetComponent<DisplayCard>().cardHighlight.SetActive(false);
        UnitManager.instance.selectedCard = null;
        GameObject.Find("Camera Pivot").GetComponent<CameraMove>().selecting = false;
    }

    public void CheckStatus()
    {
        if(isCorrupted > 0)
        {
            health -= 5 * isCorrupted;
        }

        else if (isStunned)
        {
            isStunned = false;
        }
    }
}
