using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StealthEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask wallLayer;

    [Header("Patrol State Variables")]
    public Transform[] patrolSpots;
    public Transform currentPatrolSpot;

    [Header("Investigation State Variables")]
    public Vector3 investigationTarget;
    [SerializeField] private float investigationTimer = 3f;
    private bool isInvestigating = false;
    float dot;

    [Header("Pursuit State Variables")]
    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private float updateTimer = 0;

    [Header("Separation Settings")]
    public float separationRadius = 2f;
    public float separationStrength = 3f;
    public LayerMask enemyLayer;

    [SerializeField] NavMeshAgent agent;
    private static readonly Collider[] neighborBuffer = new Collider[16];

    public State state = State.Patrol;

    public enum State
    {
        Patrol,
        Investigate,
        Pursuit
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        currentPatrolSpot = patrolSpots[0];
    }
    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        switch (state)
        {
            case State.Patrol:
                PatrolUpdate();
                break;
            case State.Investigate:
                InvestigateUpdate();
                break;
            case State.Pursuit:
                PursuitUpdate();
                break;
        }
    }

    private void PatrolUpdate()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval;
            agent.SetDestination(currentPatrolSpot.position);
        }
    }

    private void InvestigateUpdate()
    {
        agent.ResetPath(); //This clears the destination! So no more moving for now!
        transform.LookAt(investigationTarget);
        agent.SetDestination(investigationTarget);
        if (!isInvestigating) //This way, this block of code doesn't need to be running over and over, plus it doesn't repeat the coroutine!
        {
            isInvestigating = true;
            StartCoroutine(Investigate()); //use Coroutine for timer
        }
        SearchForPlayer();
    }

    IEnumerator Investigate()
    {
        //Debug.Log("Investigating...");
        yield return new WaitForSeconds(investigationTimer);
        state = State.Patrol;
        isInvestigating = false;
    }

    private void SearchForPlayer()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 toPlayer = (player.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDirection, toPlayer);

        if (dot > 0.8 && !IsThereAWall())
        {
            //Debug.Log("Chase time!");
            state = State.Pursuit;
            StopAllCoroutines();
        }
    }

    public bool IsThereAWall() //Returns TRUE if there IS a wall.
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer, wallLayer)) //Need to look into what out RaycastHit is
        {
            // If the ray hits something in the obstruction mask before reaching the player, a wall is blocking view
            //Debug.Log("Wall in the way");
            return true;
        }
        return false;
    }

    private void PursuitUpdate()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval;
            agent.SetDestination(player.position);

            float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceFromPlayer >= GetComponentInChildren<SphereCollider>().radius || IsThereAWall()) //The EnemySenses radius
            {
                state = State.Investigate;
                Vector3 lastKnownPosition = player.position;
                investigationTarget = lastKnownPosition;
                StartCoroutine(Investigate());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("gottem");
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Should reload current scene if the player gets caught
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);

        if (dot > 0.8)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Vector3 endPoint = transform.position + transform.forward * 5;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
