using UnityEngine;
using UnityEngine.InputSystem;


[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private ApplicationActions Actions; //this player's own input actions


    public bool IsCrouching { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSprinting { get; private set; }
    
    public bool IsWalking { get; private set; }
    public Vector3 Velocity { get {return characterController.velocity; } }

    private Vector2 movementInput;
    private Vector3 movementVelocity;
    private Vector3 movement;
    private float currentSpeed;

    [Header("Settings")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CrouchSpeed;
    [SerializeField] private float CrouchHeight;
    [SerializeField] private float JumpHeight;
    [SerializeField] private float gravity = 9.81f;
    private float NormalHeight;

    private void Awake()
    {
       characterController = GetComponent<CharacterController>();

        NormalHeight = characterController.height;
        currentSpeed = WalkSpeed;
        
    }

    // replacement for OnEnable
    public void Initialise(ApplicationActions assignedActions)
    {
        Actions = assignedActions;

        Actions.Game.Move.performed += OnMove;
        Actions.Game.Move.canceled += OnMoveCancel;
        Actions.Game.Jump.performed += OnJump;
        Actions.Game.Crouch.performed += OnCrouch;
        Actions.Game.Sprint.performed += OnSprint;
        Actions.Game.Sprint.canceled += OnSprintCancel;
    }

    private void OnDisable()
    {
        if (Actions == null) return; //prevents null reference error if player is disabled before initialise()

        Actions.Game.Move.performed -= OnMove;
        Actions.Game.Move.canceled -= OnMoveCancel;
        Actions.Game.Jump.performed -= OnJump;
        Actions.Game.Crouch.performed -= OnCrouch;
        Actions.Game.Sprint.performed -= OnSprint;
        Actions.Game.Sprint.canceled -= OnSprintCancel;

    }


    public void OnMove(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>();
    }

    public void OnMoveCancel(InputAction.CallbackContext obj)
    {
        movementInput = Vector2.zero;
    }

    public bool CanJump()
    {
        return characterController.isGrounded;
    }
    public void OnJump(InputAction.CallbackContext obj)
    {
        if (CanJump())
        {
            movementVelocity.y = JumpHeight;
        }
    }

    public void OnCrouch(InputAction.CallbackContext obj)
    {
        IsCrouching = !IsCrouching;
        characterController.height = IsCrouching ? CrouchHeight : NormalHeight;
        currentSpeed = IsCrouching ? CrouchSpeed : WalkSpeed;
    }

    public void OnSprint(InputAction.CallbackContext obj)
    {
        IsSprinting = !IsSprinting;

        if (!IsCrouching)
        {
            currentSpeed = IsSprinting ? SprintSpeed : WalkSpeed;
        }
    }

    public void OnSprintCancel(InputAction.CallbackContext obj)
    {
        IsSprinting = !IsSprinting;
        currentSpeed = WalkSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (characterController.isGrounded && movementVelocity.y <0)
        {
            movementVelocity.y = -2f;
        }

        
            movement = transform.right * movementInput.x + transform.forward * movementInput.y;

            movementVelocity.y -= gravity * Time.deltaTime;


            characterController.Move(((movement * currentSpeed) + movementVelocity) * Time.deltaTime);

        
        
    }
}
