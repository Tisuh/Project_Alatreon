using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private Transform detectedTarget;
    [SerializeField] [Range(0, 360)] private int visionAngle;

    private NavMeshAgent agent;
    private Transform chaseTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (waypoints.Length <= 0)
        {
            waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoints");
        }
    }

    void Update()
    {
        Detect();
        CheckVisionRange();

        if (chaseTarget != null)
        {
            ChaseTarget();
        }
        else
        {
            Wander();
        }
    }

    void PickRandomPoint()
    {
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length)].transform.position);
    }

    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            PickRandomPoint();
        }
    }

    void Detect()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange, detectionLayers); // Detecta al jugador
        if (detectedObjects.Length > 0)
        {
            detectedTarget = detectedObjects[0].transform;
        }
        else if (detectedTarget != null)
        {
            detectedTarget = null;

            if (chaseTarget != null) // Cuando se aleja del jugador, deja de perseguirlo
            {
                chaseTarget = null;

                PickRandomPoint(); // Busca un nuevo punto al que moverse
            }
        }
    }

    void CheckVisionRange()
    {
        if (detectedTarget == null || chaseTarget != null) return;

        Vector3 direction = detectedTarget.position - transform.position; // Calcular dirección hacia el jugador

        float dot = Vector3.Dot(transform.position, direction); // Calcular el producto escalar

        if (dot >= Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad))
        {
            // Asignar el objeto a perseguir
            chaseTarget = detectedTarget;
        }
    }

    void ChaseTarget()
    {
        agent.SetDestination(chaseTarget.position);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.navajoWhite;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
    }
}
