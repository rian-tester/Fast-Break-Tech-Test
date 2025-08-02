using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("State")]
    [SerializeField, ReadOnly]
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
                transform.SetParent(null);
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
                transform.SetParent(null);
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

            default:
                break;
        }
    }

    private float GiveRandomFloat()
    {
        return Random.Range(1, 2);
    }

}
