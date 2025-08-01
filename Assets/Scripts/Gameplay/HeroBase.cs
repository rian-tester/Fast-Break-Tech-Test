using UnityEngine;
using UnityEngine.InputSystem;

public abstract class HeroBase : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 lastMoveDirection;
    private float moveSpeed = 5f;
    private Vector2 inputDirection;
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
        Vector3 moveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveDir.normalized;
            characterController.Move(lastMoveDirection * Time.deltaTime * moveSpeed);
        }
        Turn();       
    }
    
    public void Turn()
    {
        if (lastMoveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
