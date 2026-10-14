using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable, IFocusable
{

    //focus code
    public Color highlightColor = Color.white;
    private Color originalColor;
    private Color LockedColor = Color.yellow;
    private Color openColor = Color.green;
    private MeshRenderer meshRenderer;
    private Material material;

    [SerializeField] private Item item;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            material = meshRenderer.material;
        }
    }

    public void Focus(GameObject interactor)
    {
        if (material != null && !IsOpen)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", LockedColor);
            material.EnableKeyword("_EMISSION");
        }
        if (material != null && IsOpen)
        {
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", openColor);
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


    //interaction code

    public bool IsOpen = false;
    public bool CanInteractWith(GameObject Interactor)
    {
        switch (IsOpen)
        {
            case false:
                return true;
            default:
                return false;
        }
    }
    
    public void Interact(GameObject Interactor)
    {
        if (IsOpen == false)
        {
            Debug.Log($"Interacted qith {gameObject.name} by {Interactor.name}. {gameObject.name} is opened!");
            IsOpen = true;

            //sets colour immediately instead of after unfocus/focus
            originalColor = material.GetColor("_EmissionColor");
            material.SetColor("_EmissionColor", openColor);
            material.EnableKeyword("_EMISSION");

            //add item to inventory
            if (Interactor.TryGetComponent<UpdatedInventory>(out UpdatedInventory inventory))
            {
                Debug.Log($"{Interactor.name} looted the {gameObject.name}");
                inventory.AddItem(item, 1);
            }



        }

        
        else if (IsOpen)
            Debug.Log($"{gameObject.name} already opened");
        
    }

}
