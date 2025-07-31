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

            }
            float bounce = Mathf.Abs(Mathf.Sin(Time.time * BounceInterval)) * BounceHeight;
            BallOrigin.position = currentBallHandler.GetDribbleOrigin().position + Vector3.up * bounce;

        }

        if (state == BallState.Free)
        {
            //if (BallRigidbody.linearVelocity.magnitude < Mathf.Epsilon && BallRigidbody.angularVelocity.magnitude < Mathf.Epsilon) SetState(BallState.Idle);
            Debug.Log(BallRigidbody.angularVelocity);
        }

    }

    void OnCollisionEnter(Collision collision)
    {


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
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player is holding the ball");
    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Player is releasing the ball");
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
                
                BallRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(),GiveRandomFloat());

                break;
            case BallState.Taken:
                BallRigidbody.isKinematic = false;
                BallCollider.isTrigger = true;
                BallRigidbody.mass = 0;
                BallRigidbody.useGravity = false;

                BallRigidbody.AddTorque(GiveRandomFloat(), GiveRandomFloat(),GiveRandomFloat());
                Debug.Log($"The ball currently taken by {currentBallHandler.GetCharacterName()}");

                break;
        }
    }

    private float GiveRandomFloat()
    {
        return Random.Range(1, 2);
    }

}
