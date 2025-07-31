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

    private SphereCollider BallCollider;
    private Rigidbody BallRigidbody;
    private Vector3 ballOriginInitialPosition;


    void Awake()
    {
        BallCollider = GetComponent<SphereCollider>();
        BallRigidbody = GetComponent<Rigidbody>();
        if (BallOrigin != null)
            ballOriginInitialPosition = BallOrigin.position;
    }

    void Update()
    {
        if (BallRigidbody.linearVelocity.magnitude < Mathf.Epsilon && BallRigidbody.angularVelocity.magnitude < Mathf.Epsilon) SetState(BallState.Idle);

        if (state == BallState.Taken)
        {
            float bounce = Mathf.Abs(Mathf.Sin(Time.time * BounceInterval)) * BounceHeight;
            BallOrigin.position = ballOriginInitialPosition + Vector3.up * bounce;

        }
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
                    BallCollider.isTrigger = false;
                    BallRigidbody.mass = 1;
                    BallRigidbody.useGravity = true;
                    BallRigidbody.isKinematic = false;

                    break;
                case BallState.Taken:
                    BallCollider.isTrigger = true;
                    BallRigidbody.mass = 0;
                    BallRigidbody.useGravity = false;
                    BallRigidbody.isKinematic = false;

                    break;
            }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ball is touched");
    }
}
