using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public float Damage;
    public float AttackSpeed;

    public override void Use(GameObject user)
    {
        EquipmentManager equipmentManager = user.GetComponent<EquipmentManager>();

        if ((equipmentManager))
        {
            equipmentManager.EquipItem(this);
        }
    }

}
