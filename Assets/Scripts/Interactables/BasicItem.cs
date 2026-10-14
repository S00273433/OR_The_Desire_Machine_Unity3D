using UnityEngine;


public class BasicItem : MonoBehaviour, IInteractable
{
    
    [SerializeField] private Item item;

    public bool CanInteractWith(GameObject interactor)
    {
        return true;
    }

    public void Interact(GameObject interactor)
    {
        if(interactor.TryGetComponent<UpdatedInventory>(out UpdatedInventory inventory))
        {
            inventory.AddItem(item, 1);
            Destroy(gameObject);
        }
    }

}
