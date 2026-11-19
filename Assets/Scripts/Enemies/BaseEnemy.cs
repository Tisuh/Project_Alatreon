using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    [Header("Detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private Transform detectedTarget;
    [SerializeField] [Range(0, 360)] private int visionAngle = 90;

    [Header("Combat")]
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackCooldown = 2;


    private NavMeshAgent agent;
    private Transform chaseTarget;
    private bool canAttack = true;
    private float lookAtSpeed = 10;

    private void Awake()
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

        if (agent.remainingDistance <= attackRange)
        {
            agent.isStopped = true;
            LookAtPlayer();
            if (canAttack)
            {
                Attack();
                StartCoroutine(AttackCooldownCrt());
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }

    protected abstract void Attack();

    IEnumerator AttackCooldownCrt()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void LookAtPlayer()
    {
        Vector3 directionToTarget = chaseTarget.position - transform.position; // Calcular la dirección hacia el jugador

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z)), 
            Time.deltaTime * lookAtSpeed); // Rotar al personaje
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.navajoWhite;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
    }
}
