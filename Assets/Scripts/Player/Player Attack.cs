using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerAttack : MonoBehaviour
{
    // public float WeaponDistance = 3;
    // public float WeaponDamage = 25;
    //  private RaycastHit raycastHit;
    //public LayerMask DamageableLayers;

    private float AttackUpLifeTime = 45f; //powerup lifetime

    private float AttackUpTimer = 0f;  //powerup timer
    private bool HasAttackUp = false; //powerup bool

    private bool alreadyAttacked; //cooldown bool
    
    [SerializeField]
    private float timeBetweenAttacks = .75f; //attck cooldown time

    [SerializeField]
    public GameObject MagicProjectile;

    public Transform CameraTransform;

    private ApplicationActions Actions;



    public void Initialise(ApplicationActions assignedActions)
    {
        Actions = assignedActions;

        Actions.Game.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        if (Actions == null) return; //prevents null reference error if player is disabled before initialise()
        Actions.Game.Attack.performed -= OnAttack;
    }

        private void Update() //update to count up powerup lifetime, rests attack after Lifetime runs out
    {
        ResetNormalAttack(AttackUpLifeTime);
    }

    public void OnAttack(InputAction.CallbackContext obj)
    {
        if (!alreadyAttacked) //checks if attack is on cooldown
        {
            if (HasAttackUp) //checks if player has attack up powerup, if so, shoots 5 projectiles in a + pattern
            {
                
                for (int i = -1; i <= 1; i++) //x axis of pattern
                {
                    Rigidbody rb = Instantiate(MagicProjectile, CameraTransform.position + transform.forward * 1.2f, Quaternion.identity).GetComponent<Rigidbody>();
                    Vector3 spreadDirection = Quaternion.Euler(i * 5, 0, 0) * CameraTransform.forward; // Adjust the spread angle as needed
                    rb.AddForce(spreadDirection * 32f, ForceMode.Impulse);
                }
                for (int i = -1; i <= 1; i++) //y axis of pattern
                {
                    Rigidbody rb = Instantiate(MagicProjectile, CameraTransform.position + transform.forward * 1.2f, Quaternion.identity).GetComponent<Rigidbody>();
                    Vector3 spreadDirection = Quaternion.Euler(0, i*5, 0) * CameraTransform.forward; // Adjust the spread angle as needed
                    rb.AddForce(spreadDirection * 32f, ForceMode.Impulse);
                }
            }
            else //standard attack, shoots 1 projectile straight forward
            {
                Rigidbody rb = Instantiate(MagicProjectile, CameraTransform.position + transform.forward * 1.2f, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(CameraTransform.forward * 32f, ForceMode.Impulse); //straight forward attack with no arc. to add an arc, add some upward force line too
            }

            alreadyAttacked = true; //sets attack on cooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void SetAttackUp(bool value) //sets the powerup for attacks to be used
    {
        HasAttackUp = value;
        if (HasAttackUp)
        {
            AttackUpTimer = 0f; // Reset the timer when Attack Up is activated
        }
    }
    private void ResetNormalAttack (float attackUpLifeTime) //resets the attack to normal after a powerup timer runs out
    {
        if (HasAttackUp)
        {
            AttackUpTimer += Time.deltaTime;
            if (AttackUpTimer >= attackUpLifeTime)
            {
                SetAttackUp(false);
                AttackUpTimer = 0f; //also resets when attack up turns off
            }
             
        }
    }

    private void ResetAttack() //attack cooldown
    {
        alreadyAttacked = false;
    }

    //defunct for now, but could be used for a hitscan weapon in the future
    /*private void CastRay() 
    {
        if (Physics.Raycast(
            CameraTransform.position,
            CameraTransform.forward,
            out  raycastHit,
            WeaponDistance,
            DamageableLayers))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if(damageable.IsAlive)
                {
                    damageable.ApplyDamage(WeaponDamage);
                }
            }
        }
    }*/
}
