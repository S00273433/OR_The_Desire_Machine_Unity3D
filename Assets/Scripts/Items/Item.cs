using System;
using UnityEngine;

[Serializable]
public enum ItemType {Weapon, Armor, Consumable, Quest, Junk, Other, HealthUp}

[Serializable]
public enum Rarity { Common, Rare, Legendary, Exotic, Unique}

public delegate void ItemHandler(Item item);

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField]
    public string ID { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [TextArea(3, 10)] public string Description;

    [field: SerializeField]
    public int Value { get; private set; }

    [field: SerializeField]
    public float Weight { get; private set; }

    [field: SerializeField]
    public ItemType Type { get; private set; } = ItemType.Other;

    [field: SerializeField]
    public Rarity Rarity { get; private set; } = Rarity.Common;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public bool RemoveOnUse { get; private set; } = false;

    public virtual void Use(GameObject user)
    {

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
