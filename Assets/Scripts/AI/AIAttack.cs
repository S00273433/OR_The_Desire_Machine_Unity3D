using UnityEngine;
using UnityEngine.AI;

public class AIAttack : MonoBehaviour
{
   // protected NavMeshAgent Agent;
    private AIController Controller;
    private Transform playerOne;
    private Transform playerTwo;

    [SerializeField] 
    private float attackRange = 8f;

    [SerializeField] 
    private float sightRange = 15f;

    private bool IsPlayerVisible;
    private bool IsPlayerInRange;
    private bool alreadyAttacked;

    [SerializeField]
    private float timeBetweenAttacks = 1.25f; 

    public GameObject MagicProjectile;

    private Transform targetPlayer;


    private void Awake()
    {
        Controller = GetComponent<AIController>();
        playerOne = GameObject.Find("PlayerOne").transform;
        playerTwo = GameObject.Find("PlayerTwo").transform;
       // Agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        targetPlayer =  GetClosestPlayerInRange(); //get the closest player to target
        IsPlayerVisible = targetPlayer != null && Physics.CheckSphere(transform.position, sightRange, 1); //the int 1 here is the layermask the player is on, i could use a distance check instead later. I may set Projectiles to be damageable layer so the player can shoot them down.
        IsPlayerInRange = targetPlayer != null && Physics.CheckSphere(transform.position, attackRange, 1);

        
        if (!IsPlayerVisible && !IsPlayerInRange) Controller.MoveToRandomNavMeshPoint(10f);

        if (IsPlayerVisible && !IsPlayerInRange) ChasePlayer();

        if (IsPlayerInRange && IsPlayerVisible) AttackPlayer();
    }

    private Transform GetClosestPlayerInRange() //need this for two player, without this method, the ai will shoot at p2 from any range.
    {
        float distanceToPlayerOne = playerOne ? Vector3.Distance(transform.position, playerOne.position) : Mathf.Infinity; //math Infinity: if player one is null (destroyed), set distance to infinity so it won't be targeted
        float distanceToPlayerTwo = playerTwo ? Vector3.Distance(transform.position, playerTwo.position) : Mathf.Infinity;

        Transform closestPlayer = distanceToPlayerOne < distanceToPlayerTwo ? playerOne : playerTwo; // check who is closer

        return (closestPlayer != null && Vector3.Distance(transform.position, closestPlayer.position) <= sightRange) ? closestPlayer : null; 
    }
    

    private void ChasePlayer()
    {
        //prevents null reference error if player is destroyed before chase
        if(targetPlayer != null)
        {
          Controller.MoveTo(targetPlayer.position); //move towards the targeted player
        }
        
    }

    private void AttackPlayer()
    {
        Controller.StopMovement(); //stop moving

        //prevents null reference error if player is destroyed before attack
        if (targetPlayer != null)
        {
            transform.LookAt(targetPlayer); //look at the targeted player
        }
  
        if (!alreadyAttacked) //checks if attack is on cooldown
        {
            Vector3 ProjectileSpawnPos = transform.position + transform.forward * 1f; //spawn the projectile a bit in front of the enemy to prevent it from colliding with the enemy when it fires
            Rigidbody rb = Instantiate(MagicProjectile, ProjectileSpawnPos, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse); //straight forward attack with no arc. to add an arc, add some upward force line too
            rb.AddForce(transform.up * 1.2f, ForceMode.Impulse); //sligth upwards to avoid ground collision

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
