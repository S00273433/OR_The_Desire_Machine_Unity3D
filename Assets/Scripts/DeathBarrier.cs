using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public PlayerHealth playerHealthRed;
    public PlayerHealth playerHealthBlue;
    float Damage = 400;

    public void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            if (damageable.IsAlive)
            {
                damageable.ApplyDamage(Damage); //deal 25 damage to the target. this can be changed to be more dynamic by adding a damage variable to the projectile and setting it when instantiating the projectile
                
            }
        }
    }

   

   


}
