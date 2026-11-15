using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    private NavMeshAgent agent;

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
        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            PickRandomPoint();
        }
    }

    void PickRandomPoint()
    {
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length)].transform.position);
    }
}
