using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class HeroBase : MonoBehaviour, ICharacter
{
    [Header("Movement Settings")]
    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float rotationLerpSpeed = 10f;
    [SerializeField, ReadOnly]
    protected Vector2 inputDirection;
    [SerializeField, ReadOnly]
    protected Vector3 lastMoveDirection;
    protected CharacterController characterController;


    [Header("Character Settings")]
    [SerializeField]
    protected Transform playerDribbleAnchor;
    [SerializeField, ReadOnly]
    protected BallController controlledBall;


    [Header("Velocity")]
    private Vector3 previousPosition;
    private Vector3 currentVelocity;
    public Vector2 Velocity2D => new Vector2(currentVelocity.x, currentVelocity.z);
    public float VelocityY => currentVelocity.y;


    protected virtual void Start()
    {
        controlledBall = null;
        characterController = GetComponent<CharacterController>();
        previousPosition = transform.position;
    }

    protected virtual void Update()
    {
        Move();
        Turn();
        CalculateVelocity();
    }
    protected virtual void Move()
    {
        Vector3 moveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveDir.normalized;
            characterController.Move(lastMoveDirection * Time.deltaTime * moveSpeed);
        }
    }
    protected virtual void Turn()
    {
        if (lastMoveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
        }
    }
    private void CalculateVelocity()
    {
        Vector3 delta = transform.position - previousPosition;
        currentVelocity = delta / Time.deltaTime;
        previousPosition = transform.position;
    }

    public Transform GetDribbleOrigin()
    {
        return playerDribbleAnchor;
    }

    public void SetControlledBall(BallController theBall)
    {
        controlledBall = theBall;
    }

    public string GetCharacterName()
    {
        return gameObject.name;
    }

}
