using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
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



    [Header("Character Settings")]
    [SerializeField]
    protected Team playerTeam;
    [SerializeField]
    protected Transform playerDribbleAnchor;
    [SerializeField, ReadOnly]
    protected BallController controlledBall;
    public BallController ControlledBall => controlledBall;

    [Header("Shooting & Passing")]
    [SerializeField]
    protected float passingPower = 30f;
    [SerializeField]
    protected float playerAccuracy = 80f;
    [SerializeField]
    protected float flightTimeMultiplier = 0.2f;
    [SerializeField]
    protected float arcHeightMultiplier = 0.5f;
    public float PassingPower => passingPower;
    public float PlayerAccuracy => playerAccuracy;
    public float FlightTimeMultiplier => flightTimeMultiplier;
    public float ArcHeightMultiplier => arcHeightMultiplier;


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
    [SerializeField, ReadOnly]
    protected CapsuleCollider collisionTrigger;


    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        collisionTrigger = GetComponent<CapsuleCollider>();
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

    public Transform GetDribbleOrigin()
    {
        return playerDribbleAnchor;
    }

    public string GetCharacterName()
    {
        return gameObject.name;
    }

    public Team GetTeam()
    {
        return playerTeam;
    }
    
    public abstract void SetControlledBall(BallController theBall);
}
