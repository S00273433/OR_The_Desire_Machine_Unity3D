using UnityEngine;

public class AIHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float Health = 100f;
    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public void ApplyDamage(float damageAmount)
    {        
        Health -= damageAmount;
        Debug.Log($"{gameObject.name} Health at {Health}");

        if (!IsAlive)
        {
            Debug.Log($"{gameObject.name} destroyed");
            Destroy(gameObject);
        }
    }

}
