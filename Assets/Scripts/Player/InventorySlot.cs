using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{

    public Item Item;
    public int Quantity;

    public InventorySlot()
    {

    }

    public InventorySlot(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
