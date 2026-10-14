using UnityEngine;

public class MagicProjectileBasic : MonoBehaviour
{
    private Rigidbody Rigidbody;

    
    [SerializeField]
    private int LifeTime = 3;

    [SerializeField]
    private int projectileDamage = 25;

    private void Awake()
    {
        Destroy(gameObject, LifeTime); //destroy the projectile after X seconds to prevent cluttering the scene with unused projectiles
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            if (damageable.IsAlive)
            {
                damageable.ApplyDamage(projectileDamage); //deal 25 damage to the target. this can be changed to be more dynamic by adding a damage variable to the projectile and setting it when instantiating the projectile
                Destroy(gameObject); //destroy the projectile on hit
            }
        }

        else if (other.CompareTag("Environment"))
        {
            Destroy(this.gameObject); //destroy the projectile if it hits the environment
        }
    }
}
