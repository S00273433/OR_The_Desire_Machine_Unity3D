using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public ItemButton ItemButtonPrefab;
    public Transform lstItem;
    private UpdatedInventory currentInventory;

    Dictionary<Item, ItemButton> itemButtons = new Dictionary<Item, ItemButton>();

    private void OnItemSelected(Item item)
    {
        item.Use(currentInventory.gameObject);
        currentInventory.RemoveItem(item, 1);
    }

    private void OnItemRemoved(Item item)
    {
        if (!currentInventory.HasItem(item))
        {
            
            if (itemButtons.ContainsKey(item))
            {
                itemButtons[item].OnItemSelected -= OnItemSelected;

                Destroy(itemButtons[item]);
                itemButtons.Remove(item);
            }
            else
            {
                itemButtons[item].Refresh();
            }
        }
    }

    public void ShowInventory(UpdatedInventory inventory)
    {
        currentInventory = inventory;
        itemButtons.Clear();

        foreach(InventorySlot slot in inventory.GetAllItems())
        {
            ItemButton instance = Instantiate(ItemButtonPrefab, lstItem);
            instance.SetItem(slot);

            instance.OnItemSelected += OnItemSelected;
            itemButtons[slot.Item] = instance;
        }

        currentInventory.OnItemRemoved += OnItemRemoved;
    }

    private void OnDisable()
    {
        foreach (Transform t in lstItem)
        {
            Destroy(t.gameObject);
        }

        itemButtons.Clear();

        if (currentInventory != null)
        {
            currentInventory.OnItemRemoved -= OnItemRemoved;
            currentInventory = null;
        }
    }
        
    

}
