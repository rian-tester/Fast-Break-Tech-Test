using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class HeroBase : MonoBehaviour, ICharacter
{
    [Header("State")]
    [SerializeField, ReadOnly]
    protected CharacterState characterState = CharacterState.EmptyHanded;

    [Header("Movement Settings")]
    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float rotationLerpSpeed = 10f;
    [SerializeField, ReadOnly]
    protected Vector2 inputDirection;
    [SerializeField, ReadOnly]
    protected Vector3 lastMoveDirection;
    


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

    [Header("Component References")]
    [SerializeField, ReadOnly]
    protected CharacterController characterController;
    [SerializeField, ReadOnly]
    protected Animator animator;
    protected CapsuleCollider ballDetectionCollider;


    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ballDetectionCollider = GetComponent<CapsuleCollider>();
    }
    protected virtual void Start()
    {
        controlledBall = null;
        previousPosition = transform.position;
    }

    protected virtual void Update()
    {
        Move();
        Turn();
        CalculateVelocity();
        UpdateAnimation();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (controlledBall != null)
            {
                controlledBall = null;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
         if (other.gameObject.CompareTag("Ball"))
        {
            if (controlledBall == null)
            {
                controlledBall = other.gameObject.GetComponent<BallController>();
            }
        }
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

    private void UpdateAnimation()
    {

        animator.SetFloat("Velocity", Velocity2D.magnitude);
    }

    public void SetState(CharacterState newState)
    {
        if (characterState == newState) return;
        var oldState = characterState;
        characterState = newState;
        OnStateChanged(newState, oldState);
    }

    protected virtual void OnStateChanged(CharacterState newState, CharacterState oldState)
    {
        
    }

    public Transform GetDribbleOrigin()
    {
        return playerDribbleAnchor;
    }

    public void SetControlledBall(BallController theBall)
    {
        controlledBall = theBall;
        SetState(CharacterState.Dribbling);
    }

    public string GetCharacterName()
    {
        return gameObject.name;
    }

}
