using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private ApplicationActions Actions;

    private Vector2 lookInput;
    private float mouseX;
    private float mouseY;
    private float xRotation;

    [Header("Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] protected float HorizontalSensitivity = 10;
    [SerializeField] public float VerticalSensitivity = 10;
    [SerializeField] private bool IsCursorVisible = true;

    private void Awake()
    {
        Cursor.visible = IsCursorVisible;
    }

    public void Initialise(ApplicationActions assignedActions)
    {
        Actions = assignedActions;
        Actions.Game.Look.performed += OnLook;
        Actions.Game.Look.canceled += OnLook; //when player stops looking, input value should reset to zero
    }

    private void OnDisable()
    {
        if(Actions == null) return; //prevents null reference error if player is disabled before initialise()
        Actions.Game.Look.performed -= OnLook;
        Actions.Game.Look.canceled -= OnLook;
    }

    private void OnLook(InputAction.CallbackContext obj)
    { 
        lookInput = obj.ReadValue<Vector2>();
        
    }

    protected virtual void LateUpdate()
    {
        mouseX = lookInput.x * HorizontalSensitivity * Time.deltaTime;
        mouseY = lookInput.y * VerticalSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
    }
}
