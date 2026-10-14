using UnityEngine;

public class StagedEnemy : MonoBehaviour, IDamageable
{
    private MeshRenderer meshRenderer;
    private Material material;
    private Color originalColor;
    private Color Healthy = Color.green;
    private Color Damaged = Color.yellow;
    private Color Critical = Color.red;
    

    public float Health = 100;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            material = meshRenderer.material;
        }
    }

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public void ApplyDamage(float damageAmount)
    { //I should reaslly use a switch statement here instead. They're all basically the same. value ranges would be cleaner
        Health -= damageAmount;
        Debug.Log($"{gameObject.name} Health at {Health}");
        if (IsAlive && Health <= 100 && Health >=66)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", Healthy);
            material.EnableKeyword("_EMISSION");
        }
        else if (IsAlive && Health <= 66 && Health >= 33)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", Damaged);
            material.EnableKeyword("_EMISSION");

        }
        else if (IsAlive && Health <= 33 && Health >= 0)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", Critical);
            material.EnableKeyword("_EMISSION");

        }

        if (!IsAlive)
        {
            Debug.Log($"{gameObject.name} destroyed");
            Destroy(gameObject);
        }
    }


}
