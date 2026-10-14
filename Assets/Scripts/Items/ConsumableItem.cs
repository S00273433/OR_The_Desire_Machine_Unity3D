using System;
using System.Data;
using UnityEngine;

[Serializable]
public enum ConsumableType { Health, Mana, Stamina, Hunger, Thirst}

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableItem : Item
{

    public float Amount;
    public ConsumableType Stat;

    public override void Use(GameObject user)
    {
        Debug.Log($"Using {Name}, applied {Amount} to stat {Stat}");
    }
}



    
