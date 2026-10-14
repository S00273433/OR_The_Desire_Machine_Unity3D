using UnityEngine;

 [CreateAssetMenu(menuName = "Items/AttackUp")]
public class AttackUp : Item
{
    public bool HasAttackUp;
    

    private void OnEnable()
    {
        HasAttackUp = false;
    }
    

}
