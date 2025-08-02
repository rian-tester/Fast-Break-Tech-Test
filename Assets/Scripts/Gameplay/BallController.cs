using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{


    [Header("Bouncing")]
    [SerializeField]
    private float bounceInterval = 5f;
    [SerializeField]
    private float bounceHeight = 1f;
    private Transform bounceOrigin;


    [Header("Component Settings")]
    [SerializeField]
    private float rbLinearDamping = 1.2f;
    [SerializeField]
    private float rbAngularDamping = 1f;


    [Header("Component References")]
    private ICharacter currentBallHandler;
    [SerializeField, ReadOnly]
    private SphereCollider ballCollider;
    [SerializeField, ReadOnly]
    private Rigidbody ballRigidbody;
    public Rigidbody BallRigidbody => ballRigidbody;
    public BallStateMachine BallStateMachine => ballStateMachine;



    Vector3 shooterPosition = new Vector3();
    Vector3 ringTopPosition = new Vector3();
    Vector3 ringCenterPosition= new Vector3();
    Vector3 finalTarget = new Vector3();
    float shootAccuracy = 1f;
    float playerFlightTimeMultiplier = 0.2f;
    float playerArcHeightMultiplier = 0.5f;

    
    private bool isBeingPickedUp = false;
    [SerializeField]
    private BallStateMachine ballStateMachine;
    
    private float pickupCooldown = 0f;
    private const float PICKUP_COOLDOWN_TIME = 1f;

    public ICharacter GetCurrentBallHandler() => currentBallHandler;

    void Awake()
    {
        ballCollider = GetComponent<SphereCollider>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballStateMachine = GetComponent<BallStateMachine>();
        if (ballStateMachine == null)
            ballStateMachine = gameObject.AddComponent<BallStateMachine>();
    }

    void Start()
    {
        bounceOrigin = gameObject.transform;
        ballRigidbody.linearDamping = rbLinearDamping;
        ballRigidbody.angularDamping = rbAngularDamping;
        ballStateMachine.TransitionToIdle();
    }
    void Update()
    {
        if (pickupCooldown > 0)
            pickupCooldown -= Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !ballStateMachine.IsInState<BallTakenState>() && !isBeingPickedUp)
        {
            TryPickupBall(collision.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (pickupCooldown > 0) return;
        
        if (other.gameObject.CompareTag("Player") && !ballStateMachine.IsInState<BallTakenState>() && !isBeingPickedUp)
        {
            TryPickupBall(other.gameObject);
        }
    }
    
    private void TryPickupBall(GameObject playerObject)
    {
        isBeingPickedUp = true;
        
        ICharacter character = playerObject.GetComponent<ICharacter>();
        if (character != null)
        {
            currentBallHandler = character;
            character.SetControlledBall(this);
            if (currentBallHandler != null)
            {
                ballStateMachine.TransitionToTaken();
                transform.SetParent(character.GetDribbleOrigin());
            }
        }
        
        isBeingPickedUp = false;
    }
 
    public void SetupShootingTarget(Vector3 targetPosition, float accuracy, float flightMultiplier, float arcMultiplier)
    {
        shooterPosition = transform.position;
        ringTopPosition = targetPosition + Vector3.up * 0.6f;
        ringCenterPosition = targetPosition;
        shootAccuracy = accuracy;
        playerFlightTimeMultiplier = flightMultiplier;
        playerArcHeightMultiplier = arcMultiplier;
        finalTarget = ApplyAccuracyVariance(ringTopPosition, shootAccuracy);
        
    }
    
    private Vector3 ApplyAccuracyVariance(Vector3 perfectTarget, float accuracy)
    {
        if (accuracy >= 100f) return perfectTarget;
        
        float maxDeviation = Mathf.Lerp(2f, 0.01f, accuracy / 100f);
        Vector3 randomOffset = Random.insideUnitCircle * maxDeviation;
        return perfectTarget + new Vector3(randomOffset.x, 0, randomOffset.y);
    }
    
    private IEnumerator FlyToRingCoroutine(Vector3 startPos, Vector3 targetPos, Vector3 ringCenter, float flightTimeMultiplier, float arcHeightMultiplier)
    {
        float distance = Vector3.Distance(startPos, targetPos);
        float flightTime = distance * flightTimeMultiplier;
        float arcHeight = (distance * arcHeightMultiplier) + (3f / Mathf.Max(distance, 1f));
        
        Debug.Log($"Ball Flight - Time: {flightTime}, Arc: {arcHeight}, Distance: {distance}");
        
        float elapsedTime = 0f;
        
        while (elapsedTime < flightTime)
        {
            float t = elapsedTime / flightTime;
            
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            currentPos.y += heightOffset;
            
            transform.position = currentPos;
            transform.Rotate(Vector3.right * 360f * Time.deltaTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPos;
        
        bool hitRing = Vector3.Distance(targetPos, ringCenter + Vector3.up * 0.6f) < 0.5f;
        
        if (hitRing)
        {
            yield return new WaitForSeconds(0.1f);
            
            float dropTime = 0.3f;
            Vector3 dropStart = transform.position;
            elapsedTime = 0f;
            
            while (elapsedTime < dropTime)
            {
                float t = elapsedTime / dropTime;
                transform.position = Vector3.Lerp(dropStart, ringCenter, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            transform.position = ringCenter;
        }

        
        shooterPosition = new Vector3();
        ringTopPosition = new Vector3();
        ringCenterPosition= new Vector3();
        finalTarget = new Vector3();
        shootAccuracy = 1f;
        playerFlightTimeMultiplier = 0.2f;
        playerArcHeightMultiplier = 0.5f;

        
        Debug.Log("Ball shooting sequence complete - ball state updated to Free");
        
        if (ballStateMachine.CurrentState is BallFlyToRingState flyState)
        {
            flyState.OnFlightComplete();
        }
    }


    public float GiveRandomFloat()
    {
        return Random.Range(1, 2);
    }

    public void ClearBallHandler()
    {
        currentBallHandler = null;
    }

    public void SetBallHandler(ICharacter handler)
    {
        currentBallHandler = handler;
    }

    public void SetPickupCooldown()
    {
        pickupCooldown = PICKUP_COOLDOWN_TIME;
    }

    public void UpdateBouncing(float deltaTime)
    {
        if (currentBallHandler == null)
            return;

        float bounce = Mathf.Abs(Mathf.Sin(Time.time * bounceInterval)) * bounceHeight;
        
        Vector3 dribblePosition = currentBallHandler.GetDribbleOrigin().position;
        transform.position = dribblePosition + Vector3.up * bounce;
    }    public void StartFlightCoroutine()
    {
        StartCoroutine(FlyToRingCoroutine(shooterPosition, finalTarget, ringCenterPosition, playerFlightTimeMultiplier, playerArcHeightMultiplier));
    }

    [ContextMenu("Test Ball State Machine")]
    public void TestBallStateMachine()
    {
        if (!Application.isPlaying) return;
        
        Debug.Log($"=== Testing Ball State Machine for {name} ===");
        Debug.Log($"Current state: {ballStateMachine.CurrentState.GetType().Name}");
        
        ballStateMachine.TransitionToFree();
        Debug.Log("Transitioned to Free state");
        
        ballStateMachine.TransitionToIdle();
        Debug.Log("Transitioned to Idle state");
    }

    [ContextMenu("Force Ball Free")]
    public void ForceBallFree()
    {
        if (!Application.isPlaying) return;
        ballStateMachine.TransitionToFree();
    }

}
