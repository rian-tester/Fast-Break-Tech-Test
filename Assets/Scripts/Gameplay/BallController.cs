using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("State")]
    [SerializeField]
    private BallState state = BallState.Idle;

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
    public BallState State => state;



    Vector3 shooterPosition = new Vector3();
    Vector3 ringTopPosition = new Vector3();
    Vector3 ringCenterPosition= new Vector3();
    Vector3 finalTarget = new Vector3();
    float shootAccuracy = 1f;
    float flightTimeMultiplier = 0.2f;
    float arcHeightMultiplier = 0.5f;

    
    private bool isBeingPickedUp = false;

    void Awake()
    {
        ballCollider = GetComponent<SphereCollider>();
        ballRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        bounceOrigin = gameObject.transform;
        ballRigidbody.linearDamping = rbLinearDamping;
        ballRigidbody.angularDamping = rbAngularDamping;
        SetState(BallState.Idle);
    }
    void Update()
    {
        if (state == BallState.Taken)
        {
            if (currentBallHandler == null)
            {
                Debug.LogWarning("There is no ball handler, cant bounce!");
            }
            else
            {

            }
            float bounce = Mathf.Abs(Mathf.Sin(Time.time * bounceInterval)) * bounceHeight;
            bounceOrigin.position = currentBallHandler.GetDribbleOrigin().position + Vector3.up * bounce;

        }

        if (ballRigidbody.linearVelocity.magnitude < Mathf.Epsilon && ballRigidbody.angularVelocity.magnitude < Mathf.Epsilon) SetState(BallState.Idle);
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && state != BallState.Taken && !isBeingPickedUp)
        {
            TryPickupBall(collision.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && state != BallState.Taken && !isBeingPickedUp)
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
                SetState(BallState.Taken);
                transform.SetParent(playerObject.transform);
            }
        }
        
        isBeingPickedUp = false;
    }
 
    public void SetupShootingTarget(Vector3 targetPosition, float accuracy)
    {
        shooterPosition = transform.position;
        ringTopPosition = targetPosition + Vector3.up * 0.6f;
        ringCenterPosition = targetPosition;
        shootAccuracy = accuracy;
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

        //SetState(BallState.Free);
        shooterPosition = new Vector3();
        ringTopPosition = new Vector3();
        ringCenterPosition= new Vector3();
        finalTarget = new Vector3();
        shootAccuracy = 1f;

        
        Debug.Log("Ball shooting sequence complete - ball state updated to Free");
    }
    public void SetState(BallState newState)
    {
        state = newState;
        isBeingPickedUp = false;
        
        switch (state)
        {
            case BallState.Idle:
                ballRigidbody.isKinematic = false;
                ballCollider.isTrigger = false;
                ballRigidbody.mass = 1f;
                ballRigidbody.useGravity = true;
                if (transform.parent != null) transform.SetParent(null);
                ballRigidbody.WakeUp();
                if (currentBallHandler != null)
                {
                    currentBallHandler = null;
                }
                
                

                break;
            case BallState.Free:
                ballRigidbody.isKinematic = false;
                ballCollider.isTrigger = false;
                ballRigidbody.mass = 1;
                ballRigidbody.useGravity = true;
                if (transform.parent != null) transform.SetParent(null);
                ballRigidbody.WakeUp();
                if (currentBallHandler != null)
                {
                    currentBallHandler = null;
                }

                ballRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(), GiveRandomFloat());

                break;
            case BallState.Taken:
                ballRigidbody.isKinematic = false;
                ballCollider.isTrigger = true;
                ballRigidbody.mass = 0;
                ballRigidbody.useGravity = false;
                ballRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(), GiveRandomFloat());
                Debug.Log($"The ball currently taken by {currentBallHandler.GetCharacterName()}");

                break;
            
            case BallState.FlyToRing:
                ballRigidbody.isKinematic = true;
                ballCollider.isTrigger = true;
                ballRigidbody.mass = 0f;
                ballRigidbody.useGravity = false;
                if (transform.parent != null) transform.SetParent(null);
                if (currentBallHandler != null)
                {
                    currentBallHandler = null;
                }

                ballRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(), GiveRandomFloat());
                StartCoroutine(FlyToRingCoroutine(shooterPosition, finalTarget, ringCenterPosition, flightTimeMultiplier, arcHeightMultiplier));
                
                break;

            default:
                break;
        }
    }

    private float GiveRandomFloat()
    {
        return Random.Range(1, 2);
    }

}
