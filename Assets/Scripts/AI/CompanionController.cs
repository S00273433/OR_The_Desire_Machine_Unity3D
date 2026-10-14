using Unity.VisualScripting;
using UnityEngine;

public class CompanionController : AIController
{

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float resumeFollowDistance = 3f;

    private bool isFollowing;
    private Vector3 lastPlayerPosition;

    protected override void Update()
    {
        base.Update(); //must always be called in child classes

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            HandleFollowThreshold(distanceToPlayer);
        }
    }

    private void HandleFollowThreshold(float distanceToPlayer) //follows after set distance
    {
        if (!isFollowing)
        {
            if(distanceToPlayer > resumeFollowDistance)
            {
                isFollowing = true;
                lastPlayerPosition = playerTransform.position;
                MoveTo(playerTransform.position);
            }
        }
        else
        {
            float playerMoveDelta = Vector3.Distance(playerTransform.position, lastPlayerPosition);

            if(playerMoveDelta > Agent.stoppingDistance)
            {
                lastPlayerPosition = playerTransform.position;
                MoveTo(playerTransform.position);
            }
        }
    }

    private void OnEnable()
    {
         OnReachedDestination += CompanionController_OnReachedDestination;
    }

    private void OnDisable()
    {
        OnReachedDestination -= CompanionController_OnReachedDestination;
    }

    private void CompanionController_OnReachedDestination()
    {
        isFollowing = false;
    }
}
