using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private BallState state = BallState.Idle;
    [SerializeField]
    private float BounceInterval = 5f;
    [SerializeField]
    private float BounceHeight = 1f;

    public Transform BallOrigin;

    private ICharacter currentBallHandler;
    private SphereCollider BallCollider;
    private Rigidbody BallRigidbody;
    private Vector3 ballOriginInitialPosition;


    void Awake()
    {
        BallCollider = GetComponent<SphereCollider>();
        BallRigidbody = GetComponent<Rigidbody>();
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
                float bounce = Mathf.Abs(Mathf.Sin(Time.time * BounceInterval)) * BounceHeight;
                BallOrigin.position = currentBallHandler.GetDribbleOrigin().position + Vector3.up * bounce;
            }
            
        }

        if (state == BallState.Idle) return;
        if (BallRigidbody.linearVelocity.magnitude < Mathf.Epsilon && BallRigidbody.angularVelocity.magnitude < Mathf.Epsilon) SetState(BallState.Idle);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ball is touched by player");

            ICharacter hero = collision.gameObject.GetComponent<ICharacter>();
            if (hero != null)
            {
                currentBallHandler = hero;
                hero.SetControlledBall(this);
            }
            transform.SetParent(collision.gameObject.transform);
            SetState(BallState.Taken);
        }

    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Player is holding the ball");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player is releasing the ball");
    }


    public void SetState(BallState state)
    {
        this.state = state;
        switch (state)
        {
            case BallState.Idle:
                BallRigidbody.isKinematic = true;

                break;
            case BallState.Free:
                BallRigidbody.isKinematic = false;
                BallCollider.isTrigger = false;
                BallRigidbody.mass = 1;
                BallRigidbody.useGravity = true;
                transform.SetParent(null);


                break;
            case BallState.Taken:
                BallRigidbody.isKinematic = false;
                BallCollider.isTrigger = true;
                BallRigidbody.mass = 0;
                BallRigidbody.useGravity = false;

                break;
        }
    }

}
