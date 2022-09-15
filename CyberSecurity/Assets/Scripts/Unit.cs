using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int id;
    public int speed;
    public int health;
    public int maxHealth;
    public int baseMovementSpeed;
    public int movementSpeed;

    public bool isAlive;
    public bool isDetected;
    public bool inAction;    

    public bool isThrottled = false;
    public bool isBuffed = false;
    public bool isStunned = false;
    public bool isCorrupted = false;
    public bool isSlowed = false;

    public int throttled = 0;
    public int buffed = 0;
    public int stun = 0;
    public int corrupt = 0;
    public int slow = 0;

    public GameObject stunEffect;
    public GameObject capEffect;
    public GameObject corruptEffect;
    public GameObject pointer;
    public GameObject aura;
    public GameObject virus;
    public GameObject turnOrderDisplay;

    public List<Node> _attackTiles;
    public Animator anim;
    public GameManager gameManager;

    PathRequestManager request;    
    Vector3[] path;
    int targetIndex;

    private void Awake()
    {
        request = GameObject.Find("Grid").GetComponent<PathRequestManager>();
        anim = GetComponent<Animator>();
        isAlive = true;
        maxHealth = health;
        movementSpeed = baseMovementSpeed;
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
        
        if (UnitManager.instance.slaughtered >= UnitManager.instance.murderGoals && UnitManager.instance.objective1 && UnitManager.instance.objective2)
        {
            FindObjectOfType<GameManager>().WinGame(); 
        }
    }

    public HashSet<Node> Select(bool ignoreObstacle, int radius)
    {
        return request.HighlightMovement(transform.position, ignoreObstacle, radius);
    }

    public void Move(Vector3 targetPos)
    {
        inAction = true;
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
                        inAction = false;
                        yield break;
                    }
                }

                else
                {
                    if (targetIndex >= movementSpeed)
                    {
                        anim.SetBool("Walk", false);
                        inAction = false;
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
        if (!isAlive)
        {
            return;
        }

        if (corrupt > 0)
        {
            foreach (Node n in UnitManager.instance.grid.GetNeighbours(UnitManager.instance.grid.NodeFromWorldPoint(transform.position), 1))
            {
                if (n.ReturnObject() != null)
                {
                    if (n.ReturnObject().CompareTag("Security Control") && n.ReturnObject().GetComponent<Unit>().isAlive)
                    {
                        n.ReturnObject().GetComponent<Unit>().corrupt += 1;
                        Instantiate(corruptEffect, n.ReturnObject().transform);
                    }
                }
            }

            if (corrupt < 7)
            {
                isCorrupted = true;
                corrupt++;
                health -= 3 * corrupt;
            }

            else if (corrupt == 7)
            {
                health -= 3 * corrupt;
                List<Node> nodesInRange = UnitManager.instance.grid.
                GetNeighbours(UnitManager.instance.grid.NodeFromWorldPoint(transform.position), 1);

                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() == null)
                    {
                        GameObject virusClone = Instantiate(virus, UnitManager.instance.unitGroup.transform);
                        virusClone.transform.position = n.worldPos;
                        virusClone.layer = 3;
                        UnitManager.instance.SortTurnOrder();

                        for (int i = 0; i < corrupt; i++)
                        {
                            Destroy(transform.Find("CorruptionEffect(Clone)").gameObject);
                        }

                        corrupt = 0;
                        isCorrupted = false;
                        break;
                    }
                }
            }
        }

        if (transform.CompareTag("Malware"))
        {
            if (stun > 0)
            {
                isStunned = true;
                stun--;
                UnitManager.instance.Invoke("EndTurn", 1);
            }

            else
            {
                isStunned = false;
                stunEffect.SetActive(false);
            }
        }        

        if (slow > 0)
        {
            if (!isSlowed)
            {
                movementSpeed = baseMovementSpeed / 2;
                isSlowed = true;
            }
            
            slow--;                       
        }

        else
        {
            isSlowed = false;
            movementSpeed = baseMovementSpeed;
        }

        if (throttled > 0)
        {
            if (!isThrottled)
            {
                movementSpeed++;
                isThrottled = true;
            }

            throttled--;
        }

        else
        {
            isThrottled = false;
            movementSpeed = baseMovementSpeed;
            if (transform.Find("SpeedUp(Clone)") != null)
            {
                Destroy(transform.Find("SpeedUp(Clone)").gameObject);
            }
        }

        if (health <= 0)
        {
            anim.SetTrigger("Dead");

            for (int i = 0; i < corrupt; i++)
            {
                Destroy(transform.Find("CorruptionEffect(Clone)").gameObject);
            }

            isThrottled = false;
            isBuffed = false;
            isStunned = false;
            isCorrupted = false;
            isSlowed = false;

            throttled = 0;
            buffed = 0;
            stun = 0;
            corrupt = 0;
            slow = 0;

            isAlive = false;
        }
    }
}
