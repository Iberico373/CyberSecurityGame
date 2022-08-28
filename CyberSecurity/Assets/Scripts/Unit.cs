using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Unit : MonoBehaviour
{
    public int id;
    public int speed;
    public int health;
    public int maxHealth;
    public int movementSpeed;

    public bool isDetected;
    public bool startTurn;
    public bool isAlive;
    public int isThrottled;
    public int adEffect;
    public bool isBuffed;
    public bool isStunned;
    public int isCorrupted;

    public Animator anim;
    public GameObject pointer;
    public GameObject aura;
    public GameObject turnOrderDisplay;
    public List<Node> _attackTiles;
    public GameManager gameManager;
    
    PathRequestManager request;    
    Vector3[] path;
    int targetIndex;

    private void Awake()
    {
        request = GameObject.Find("Grid").GetComponent<PathRequestManager>();
        anim = GetComponent<Animator>();
        maxHealth = health;
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (gameObject.CompareTag("Objective"))
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }
        
        if (GameObject.FindWithTag("Malware"))
        {
            return;
        }
        
        else
        {           
            FindObjectOfType<GameManager>().WinGame();            
        }
    }

    public HashSet<Node> Select(bool ignoreObstacle)
    {
        return request.HighlightMovement(transform.position, ignoreObstacle);
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
    }

    public void CheckStatus()
    {
        if (isCorrupted > 0)
        {
            health -= 5 * isCorrupted;
        }

        else if (isStunned)
        {
            isStunned = false;
        }
    }
}
