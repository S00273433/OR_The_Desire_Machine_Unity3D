using System;
using UnityEngine;

[Serializable]
public enum ArmorSlot { Head, Arms, Body, Legs }

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorItem : Item
{
    public float Defence;
    public ArmorSlot Slot;

    public override void Use(GameObject user)
    {
        EquipmentManager equipmentManager = user.GetComponent<EquipmentManager>();

        if (equipmentManager)
        {
            equipmentManager.EquipItem(this);
        }
    }
}
