using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private float updateTimer = 0;
    public float stoppingDistance = 1f;

    [Header("Separation Settings")]
    public float separationRadius = 2f;
    public float separationStrength = 3f;
    public LayerMask enemyLayer;

    [SerializeField] NavMeshAgent agent;
    private static readonly Collider[] neighborBuffer = new Collider[16];


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; //Interacts with the Stopping Distance in the NavMesh. (It looks like all of these things must use headers and must have interactable variables as this shows up as Stopping Distance, as how our own variables show up in the inspector when serialized)

    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval;
            Vector3 destination = player.position + GetSeparationVector();

            if(NavMesh.SamplePosition(destination, out NavMeshHit hit, separationRadius + 1, NavMesh.AllAreas)) //I don't fully know what out NavMesh hit is, must look into.
                agent.SetDestination(hit.position);
            else
                agent.SetDestination(destination);
        }
    }

    private Vector3 GetSeparationVector() //Need to figure out this math on my own time
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, separationRadius, neighborBuffer, enemyLayer);
        Vector3 push = Vector3.zero;
        int neighbors = 0;

        for (int i = 0; i < count; i++)
        {
            Collider other = neighborBuffer[i];
            if (other.transform == transform)
                continue;
            Vector3 difference = transform.position - other.transform.position;
            float distance = difference.magnitude;
            if (distance > 0.001f && distance < separationRadius)
            {
                push += difference.normalized * (separationRadius - distance);
                neighbors++;
            }
        }
        if (neighbors > 0)
        {
            push /= neighbors;
            push *= separationStrength;
        }
        return push;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
