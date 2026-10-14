using UnityEngine;
using UnityEngine.InputSystem;

//This class handles interactions, it is in effect PlayerInteractions, but i named it wrong in the labs. Also handles focus
public interface IInteractable
{
    public bool CanInteractWith(GameObject interactor);
    public void Interact(GameObject interactor);
}

public interface IFocusable
{
    public void Focus(GameObject interactor);
    public void Unfocus(GameObject interactor);
}


public class PlayerBehaviour : MonoBehaviour
{
    public Transform CameraTransform;
    public float InteractionDDistance = 3;
    public LayerMask InteractionLayers;
    public RaycastHit RaycastHit;

    private IInteractable currentInteractable;
    private IFocusable currentFocusable;

    private ApplicationActions Actions;
    public void Initialise(ApplicationActions assignedActions)
    {
        Actions = assignedActions;

        Actions.Game.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        if (Actions == null) return; //prevents null reference error if player is disabled before initialise()
        
        Actions.Game.Interact.performed -= OnInteract;
        ClearFocus();
    }

    public void OnInteract(InputAction.CallbackContext obj)
    {
        if (currentInteractable != null)
        {
            {
                if (currentInteractable.CanInteractWith(gameObject))
                {
                    currentInteractable.Interact(gameObject);
                }
            }
        }
    }

    private void Update()
    {
        CastRay();
    }

    private void CastRay()
    {
        if (Physics.Raycast(
            CameraTransform.position,
            CameraTransform.forward,
            out RaycastHit,
            InteractionDDistance,
            InteractionLayers))
        {
            RaycastHit.collider.gameObject.TryGetComponent<IInteractable>(out currentInteractable);
            RaycastHit.collider.gameObject.TryGetComponent<IFocusable>(out IFocusable newFocusable);

            if (newFocusable != currentFocusable)
            {
                ClearFocus();

                currentFocusable = newFocusable;

                if (currentFocusable != null)
                    currentFocusable.Focus(gameObject);
            }
                
        }
        else
        {
            ClearFocus();
        }
    }

    private void ClearFocus()
    {
        if(currentFocusable != null && (currentFocusable as Object) != null)
            currentFocusable.Unfocus(gameObject);

        currentInteractable = null;
        currentFocusable = null;
    }
}
