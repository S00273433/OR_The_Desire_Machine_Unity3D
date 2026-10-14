using UnityEngine;


public class BreakableWindow : MonoBehaviour, IDamageable
{

    public float Health = 100;
    public bool Smashed = false;

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public void ApplyDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (IsAlive && Health > 0)
        {
            Debug.Log($"Window Health at {Health}");
        }
        if (!IsAlive)
        {
            Smashed = !Smashed;
            if (Smashed)
            {
                Debug.Log("Window Smashed!");
                Destroy(gameObject);
            }
            
            
        }
    }
}
