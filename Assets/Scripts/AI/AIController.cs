using UnityEngine;
using UnityEngine.AI;

public delegate void AiNavigationHandler();

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]

public class AIController : MonoBehaviour
{
    
    protected NavMeshAgent Agent;
    protected Vector3? currentTarget;

    public event AiNavigationHandler OnReachedDestination; //Switches for new navpoints
    public event AiNavigationHandler OnDestinationChanged;  //whenver a new navpoint is set
    public event AiNavigationHandler OnMovementFailed; //finds a new navpoint if the current one is unreachable

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public bool HasReachedDestination
    {
        get
        {
            return
                  !Agent.pathPending &&
                  currentTarget.HasValue &&
                  Agent.remainingDistance <= Agent.stoppingDistance;
        }
    }

    public bool IsMoving
    {
        get
        {
            return
                !Agent.isStopped &&
                Agent.velocity.sqrMagnitude > 0.01f;
        }
    }

    protected virtual void Update()
    {
      
        if(Agent.isStopped)
        {
           if (HasReachedDestination)
            {
                Agent.isStopped = true;
                currentTarget = null;
                OnReachedDestination?.Invoke();
            }
        }
    }

    public bool MoveTo(Vector3 target) //first compares to navmesh, if not valid, then invokes OnMovementFailed
    {
        bool success = Agent.SetDestination(target);

        if (success)
        {
            currentTarget = target;
            Agent.isStopped = false;

            OnDestinationChanged?.Invoke();
        }
        else
        {
            OnMovementFailed?.Invoke();
        }

        return success;
    }

    public bool MoveToRandomNavMeshPoint(float Radius) //randompoint on navmesh
    {
        Vector3 randomDiection = Random.insideUnitSphere * Radius + transform.position;

        if (NavMesh.SamplePosition(randomDiection, out NavMeshHit hit, Radius, NavMesh.AllAreas))
        {
            return MoveTo(hit.position);
        }
        return false;
    }

    public bool TeleportTo(Vector3 target)
    {
        return Agent.Warp(target);
    }

    public void StopMovement()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
        currentTarget = null;
    }

    public void PauseMovement()
    {
        Agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        if (currentTarget.HasValue)
        {
            Agent.isStopped = false;
        }
    }

    public void EnableAvoidance(ObstacleAvoidanceType avoidanceType, int avoidancePriority = 50)
    {
        Agent.obstacleAvoidanceType = avoidanceType;
        Agent.avoidancePriority = Mathf.Clamp(avoidancePriority, 0, 99);
    }

    public void DisableAvoidance()
    {
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }


}


