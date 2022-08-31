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

    public bool isAlive;
    public bool isDetected;
    public bool startTurn;

    public List<int> statusEffects = new List<int>();
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
    List<Node> nodesInRange;

    PathRequestManager request;    
    Vector3[] path;
    int targetIndex;

    private void Awake()
    {
        request = GameObject.Find("Grid").GetComponent<PathRequestManager>();
        anim = GetComponent<Animator>();
        maxHealth = health;

        statusEffects.Add(throttled);
        statusEffects.Add(buffed);
        statusEffects.Add(stun);
        statusEffects.Add(corrupt);
        statusEffects.Add(slow);
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
        if (health <= 0)
        {
            anim.SetTrigger("Dead");

            for (int i = 0; i < statusEffects.Count; i++)
            {
                statusEffects[i] = 0;
            }

            for (int i = 0; i < corrupt; i++)
            {
                Destroy(transform.Find("CorruptionEffect(Clone)").gameObject);
            }
        }


        if (corrupt > 0)
        {
            health -= 3 * corrupt;

            foreach (Node n in UnitManager.instance.grid.GetNeighbours(UnitManager.instance.grid.NodeFromWorldPoint(transform.position),1))
            {
                if(n.ReturnObject() != null)
                {
                    if (n.ReturnObject().CompareTag("Security Control"))
                    {
                        n.ReturnObject().GetComponent<Unit>().corrupt += 1;
                        Instantiate(corruptEffect, n.ReturnObject().transform);
                    }
                }
            }

            if (corrupt + 1 == 5)
            {
                corrupt = 0;

                nodesInRange = UnitManager.instance.grid.GetNeighbours(UnitManager.instance.grid.NodeFromWorldPoint(transform.position), 1);

                foreach (Node n in nodesInRange)
                {
                    if (n.ReturnObject() == null)
                    {
                        GameObject virusClone = Instantiate(virus, UnitManager.instance.unitGroup.transform);
                        virusClone.transform.position = n.worldPos;
                        virusClone.layer = 3;
                        UnitManager.instance.SortTurnOrder();
                        break;
                    }
                }
            }
        }

        if (stun > 0)
        {
            if (stun - 1 == 0)
            {
                stun--;
                stunEffect.SetActive(false);
                return;
            }

            stun--;
            UnitManager.instance.EndTurn();
        }

        if (slow > 0)
        {
            if (slow - 1 == 0)
            {
                slow--;
                movementSpeed = 2;
            }

            movementSpeed = 1;
            slow--;
        }        
    }
}
