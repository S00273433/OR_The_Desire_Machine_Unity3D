using UnityEngine;

public class PlayerPhysicsResponse : MonoBehaviour
{
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.attachedRigidbody)
        {
            if (!hit.collider.attachedRigidbody.isKinematic)
            {
                hit.collider.attachedRigidbody.linearVelocity += characterController.velocity;
            }
        }
    }
}
