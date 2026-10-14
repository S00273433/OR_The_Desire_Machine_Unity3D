using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Trap : MonoBehaviour, IInteractable, IDamageable, IFocusable
{
    private MeshRenderer meshRenderer;
    private Material material;
    private Color originalColor;
    private Color focusRed = Color.red;
    public float Health = 50;

    public bool IsArmed = true; //disarm
    public bool IsActive = true;

    [SerializeField] private Item item;
    public void ActiveFlag()
    {
        if (IsActive == false)
        {
            gameObject.SetActive(false);
        }

    }

    public bool IsAlive
    {
        get { return Health > 0; }
    }
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            material = meshRenderer.material;
        }
    }

    public bool CanInteractWith(GameObject Interactor)
    {
        return IsArmed && IsActive;
    }

    public void Interact(GameObject Interactor)
    {
        Debug.Log("interaction");
        if (IsArmed == true)
        {
            if (Interactor.TryGetComponent<UpdatedInventory>(out UpdatedInventory inventory))
            {
                if (inventory.HasItem(item))
                {
                    Debug.Log("Trap Disarmed");
                    IsArmed = false;
                    IsActive = false;
                }
                else
                {
                    Debug.Log("No DisarmKit!");
                }
            }
        }
    }

    public void ApplyDamage(float damageAmount)
    {
        Health -= damageAmount;
        if(IsAlive)
        {
            Debug.Log($"{gameObject.name} Health at {Health}");
            
        }
        if  (!IsAlive)
        {
            IsActive = !IsActive;
            Debug.Log($"{gameObject.name} destroyed");
        }
    }

    public void Focus(GameObject interactor)
    {
        if (material != null && IsActive)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", focusRed);
            material.EnableKeyword("_EMISSION");
        }  
    }

    public void Unfocus(GameObject interactor)
    {
        if (material != null)
        {
            material.SetColor("_EmissionColor", originalColor);
            material.DisableKeyword("_EMISSION");
        }
    }


}
