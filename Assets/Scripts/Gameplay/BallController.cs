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

        switch (state)
        {
            case BallState.Idle:

                break;
            case BallState.Free:

                break;
            case BallState.Taken:
                BallCollider.isTrigger = true;
                BallRigidbody.mass = 0;
                BallRigidbody.useGravity = false;
                BallRigidbody.isKinematic = false;

                float bounce = Mathf.Abs(Mathf.Sin(Time.time * BounceInterval)) * BounceHeight;
                BallOrigin.position = ballOriginInitialPosition + Vector3.up * bounce; 

                break;
        }
    }

    public void SetState(BallState state)
    {
        this.state = state;
    }
}
