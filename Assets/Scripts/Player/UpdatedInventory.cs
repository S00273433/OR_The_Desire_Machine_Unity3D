using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UpdatedInventory : MonoBehaviour
{
    public event ItemHandler OnItemAdded;
    public event ItemHandler OnItemRemoved;

   
    [SerializeField] private List<InventorySlot> items = new List<InventorySlot>();

    public void AddItem(Item item, int quantity)
    {
        InventorySlot existing = items.Find(slot => slot.Item == item);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            items.Add(new InventorySlot(item, quantity));
        }
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(Item item, int quantity)
    {
        InventorySlot existing = items.Find(slot => slot.Item == item);

        if (existing != null)
        {
            if (existing.Item.RemoveOnUse)
            {
                existing.Quantity -= quantity;
                if (existing.Quantity <= 0)
                {
                    items.Remove(existing);
                }
                OnItemRemoved?.Invoke(item);
            }
            
        }
    }

    public bool HasItem(Item item)
    {
        return items.Any(slot => slot.Item == item);
    }

    public int GetItemCount(Item item)
    {
        InventorySlot slot = items.Find(slot => slot.Item == item);

        if (slot == null) return 0;

        return slot.Quantity;
    }       

    public List<InventorySlot> GetAllItems()
    {
        return items;
    }
}
