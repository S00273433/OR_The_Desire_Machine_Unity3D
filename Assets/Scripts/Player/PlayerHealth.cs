using System;
using TMPro;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(float damageAmount);
    bool IsAlive { get; }
}

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public Action<GameObject> OnPlayerDeath;

    [SerializeField] private TMP_Text txtPlayerHealth;
    [SerializeField]
    private float Health = 150f;

    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private Transform player;

    [SerializeField]
    HealthUp healthUp;

    CharacterController characterController;
    public bool IsAlive
    {
        get { return Health > 0; }
    }
    private void OnEnable()
    {
        UpdateUI();
    }

    public void ApplyDamage(float damageAmount)
    {
        Health -= damageAmount;
        Debug.Log($"{gameObject.name} Health at {Health}");
        UpdateUI();

        if (!IsAlive)
        {
            Debug.Log($"{gameObject.name} destroyed");
            OnDeath();
            // OnPlayerDeath?.Invoke(this, EventArgs.Empty); //Might put death screen logic here later, but for now just respawn the player immediately
        }
    }

    public void AddHealth(float AddAmount)
    {
        Health += AddAmount;
        if (Health > 250) // means 250 is the max possible health
        {
            Health = 250;
        }
        Debug.Log($"{gameObject.name} Power up collected. Health set to {Health}");
        UpdateUI();
    }

    private void UpdateUI()
    {

        txtPlayerHealth.SetText($"Health: {Health}");
       

    }
    public void OnDeath()
    {   
        // Handle player death logic here (e.g., respawn, game over screen, etc.)
        if (!IsAlive) //respawn
        {
            OnPlayerDeath?.Invoke(gameObject); //invoke death event for any listeners (e.g., capture area)
            //Turns off the character controller to allow TP, teleports, and the turn sit back on.
            characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false; // Disable the CharacterController to prevent physics issues during teleportation
            }

            player.position = respawnPoint.position; 
            player.rotation = respawnPoint.rotation;
            Health = 150; //reset health to full on respawn
            characterController.enabled = true;
            UpdateUI();


        }

    }


}
