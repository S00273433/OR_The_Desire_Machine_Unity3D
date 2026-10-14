using UnityEngine;

public class SimpleItemComponent : MonoBehaviour, IInteractable
{
    public Item Item;
    public int Quantity = 1;

    public bool CanInteractWith(GameObject Interactor)
    {
        return true;
    }   
    public void Interact (GameObject Interactor)
    {
        UpdatedInventory inventory = Interactor.GetComponent<UpdatedInventory>();
        if (inventory && Item)
        {
            inventory.AddItem(Item, Quantity);
            Destroy(gameObject);
        }
    }
}
