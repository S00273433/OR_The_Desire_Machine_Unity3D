using UnityEngine;
using UnityEngine.InputSystem;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    [Header("Player References")]
    [SerializeField] private GameObject PlayerOne;
    [SerializeField] private GameObject PlayerTwo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //InputSystm.devices return all currently connected input devices
        //assign the first two available devices

        var devices = InputSystem.devices;

        if (devices.Count < 2)
        {
            Debug.LogWarning("Not enough input devices detected. local co-op required");
            return;
        }

        SpawnPlayer(PlayerOne, "KBM");
        SpawnPlayer(PlayerTwo, "GamePad");
    }

   

    private void SpawnPlayer(GameObject player, string scheme)
    {
        if(player)
        {
            //create inputActions for this device alone, KBM/GamePad
            ApplicationActions actions = InputManager.CreateActionForDevice(scheme);

            if(actions != null)
            {
                //inject input instance into each component that needs it
                player.GetComponent<PlayerMovement>().Initialise(actions);
                player.GetComponent<PlayerAttack>().Initialise(actions);
                player.GetComponent<PlayerLook>().Initialise(actions);
                player.GetComponent<PlayerBehaviour>().Initialise(actions);
                
            }

        }
    }


    [SerializeField] private InventoryScreen InventoryScreen;

    public void ShowInventory(UpdatedInventory inventory)
    {
        InventoryScreen.ShowInventory(inventory);
        InventoryScreen.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    public void HideInventory()
    {
        InventoryScreen.gameObject.SetActive(false);
        Cursor.visible = false;
    }
}
