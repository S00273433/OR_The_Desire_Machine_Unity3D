using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    
    public WeaponItem EquippedWeapon { get; private set; }
    public Dictionary<ArmorSlot, ArmorItem> EquippedArmor { get; private set; }

    [SerializeField] private Transform WeaponAttachPoint;
    [SerializeField] private GameObject EquippedWeaponInstance;

    public event ItemHandler OnEquipped;
    public event ItemHandler OnUnequipped;

    public void EquipItem(Item item)
    {
        switch (item.Type)
        {
            case ItemType.Weapon:
                if(EquippedWeapon != (WeaponItem)item)  
                {
                    OnUnequipped?.Invoke(EquippedWeapon);

                    EquippedWeapon = (WeaponItem)item;

                    if(EquippedWeaponInstance)
                        Destroy(EquippedWeaponInstance);

                    EquippedWeaponInstance = Instantiate(item.Prefab, WeaponAttachPoint);
                    OnEquipped?.Invoke(EquippedWeapon);
                }
                break;

                case ItemType.Armor:
                ArmorItem armor = (ArmorItem)item;

                if(EquippedArmor.ContainsKey(armor.Slot))
                {
                    if (EquippedArmor[armor.Slot] != armor)
                    {
                        OnUnequipped?.Invoke(EquippedArmor[armor.Slot]);
                        EquippedArmor[armor.Slot] = armor;
                        OnEquipped?.Invoke(EquippedArmor[armor.Slot]);
                    }

                }
                else
                {
                    EquippedArmor.Add(armor.Slot, armor);
                    OnEquipped?.Invoke(EquippedArmor[armor.Slot]);
                }
                break;
        }

    }
}
