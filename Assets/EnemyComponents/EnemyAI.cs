using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FieldOfView))]
public class EnemyAI : MonoBehaviour
{

    private const int ALARM_RATE = 5;
    public enum AIState { Idle, Patrol, Chase }
    public AIState currentState;

    public float alarmLevel;
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public Transform player;
    public float detectionRange = 10f;

    public float resetDistance = 1.5f;

    private int patrolIndex = 0;
    private float idleTimer = 0f;

    private FieldOfView FOV;
    void Start()
    {
        currentState = AIState.Idle;
        agent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
    }

    void Update()
    {
        // Safety check to prevent confusing null reference errors
        if (player == null || agent == null || patrolPoints == null || patrolPoints.Length == 0)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < resetDistance)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (FOV.canSeePlayer == true)
        {
            currentState = AIState.Chase;

        }
        else if (currentState == AIState.Chase && distanceToPlayer > detectionRange)
        {
            currentState = AIState.Patrol;
        }

        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == AIState.Chase)
        {
            alarmLevel += ALARM_RATE * Time.deltaTime;
        }
    }
    void Idle()
    {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;

        if (idleTimer >= 3f)
        {
            currentState = AIState.Patrol;
            idleTimer = 0f;
        }
    }

    void Patrol()
    {
        agent.isStopped = false;

        if (!agent.hasPath)
        {
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = AIState.Idle;
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
}