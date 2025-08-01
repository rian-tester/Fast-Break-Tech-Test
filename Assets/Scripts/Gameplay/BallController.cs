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
    private float BounceInterval = 5f;
    [SerializeField]
    private float BounceHeight = 1f;
    private Transform bounceOrigin;


    [Header("Component Settings")]
    [SerializeField]
    private float rbLinearDamping = 1.2f;
    [SerializeField]
    private float rbAngularDamping = 1f;


    [Header("References")]
    private ICharacter currentBallHandler;
    private SphereCollider BallCollider;
    private Rigidbody BallRigidbody;

    void Awake()
    {
        BallCollider = GetComponent<SphereCollider>();
        BallRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        bounceOrigin = gameObject.transform;
        BallRigidbody.linearDamping = rbLinearDamping;
        BallRigidbody.angularDamping = rbAngularDamping;
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
            float bounce = Mathf.Abs(Mathf.Sin(Time.time * BounceInterval)) * BounceHeight;
            bounceOrigin.position = currentBallHandler.GetDribbleOrigin().position + Vector3.up * bounce;

        }

        if (state == BallState.Free)
        {
            if (BallRigidbody.linearVelocity.magnitude < Mathf.Epsilon && BallRigidbody.angularVelocity.magnitude < Mathf.Epsilon) SetState(BallState.Idle);
            Debug.Log(BallRigidbody.angularVelocity);
            Debug.Log(BallRigidbody.linearVelocity);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ICharacter character = other.gameObject.GetComponent<ICharacter>();
            if (character != null)
            {
                currentBallHandler = character;
                character.SetControlledBall(this);
            }
            transform.SetParent(other.gameObject.transform);
            SetState(BallState.Taken);
        }
    }
    public void SetState(BallState state)
    {
        this.state = state;
        switch (state)
        {
            case BallState.Idle:
                //BallRigidbody.isKinematic = true;

                break;
            case BallState.Free:
                BallRigidbody.isKinematic = false;
                BallCollider.isTrigger = false;
                BallRigidbody.mass = 1;
                BallRigidbody.useGravity = true;
                transform.SetParent(null);
                if (currentBallHandler != null)
                {
                    currentBallHandler = null;
                }

                BallRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(), GiveRandomFloat());

                break;
            case BallState.Taken:
                BallRigidbody.isKinematic = false;
                BallCollider.isTrigger = true;
                BallRigidbody.mass = 0;
                BallRigidbody.useGravity = false;

                BallRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(), GiveRandomFloat());
                Debug.Log($"The ball currently taken by {currentBallHandler.GetCharacterName()}");

                break;
        }
    }

    private float GiveRandomFloat()
    {
        return Random.Range(1, 2);
    }

}
