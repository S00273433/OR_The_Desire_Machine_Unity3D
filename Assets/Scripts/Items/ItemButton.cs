using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TMP_Text txtQuantity;
    [SerializeField] private Image imgIcon;

    private InventorySlot Slot;

    public event ItemHandler OnItemSelected;

    public void Refresh()
    {
        txtName.text = Slot.Item.Name;
        txtQuantity.text = Slot.Quantity.ToString();
        imgIcon.sprite = Slot.Item.Icon;
    }

    public void SetItem(InventorySlot slot)
    {
        Slot = slot;
        Refresh();
    }

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        OnItemSelected?.Invoke(Slot.Item);
    }

}

