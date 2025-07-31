using UnityEngine;
using UnityEngine.InputSystem;

public abstract class HeroBase : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 lastMoveDirection;
    private float moveSpeed = 5f;
    private Vector2 inputDirection;
    private Vector3 playerVelocity;
    public float gravity = -9.81f;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; 
        }

        Vector3 moveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveDir.normalized;
            characterController.Move(lastMoveDirection * Time.deltaTime * moveSpeed);
        }

        playerVelocity.y += gravity * Time.deltaTime; 
    }
    
    public void Turn()
    {

    }
}
