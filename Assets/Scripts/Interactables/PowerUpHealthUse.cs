using UnityEngine;

public class PowerUpHealthUse : MonoBehaviour
{
    [SerializeField] private HealthUp healthUp;
    [SerializeField] private float RespawnTime = 45;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerOne"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthUp.HealthUpAmount);

                ToggleActive();
            }
        }
        if (other.CompareTag("PlayerTwo"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthUp.HealthUpAmount);

                ToggleActive();
            }
        }
    }

    private void ToggleActive()
    {
        Collider c = GetComponent<Collider>();
        c.enabled = !c.enabled;

        if (!c.enabled)
            Invoke("ToggleActive", RespawnTime);

        MeshRenderer[] m = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in m)
        {
            r.enabled = !r.enabled;
        }
    }


}
