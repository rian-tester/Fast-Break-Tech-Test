using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField]
    private Transform playerDribbleAnchor;

    [SerializeField]
    private BallController controlledBall;


    void Start()
    {
        controlledBall = null;
    }
    public Transform GetDribbleOrigin()
    {
        return playerDribbleAnchor;
    }

    public void SetControlledBall(BallController theBall)
    {
        controlledBall = theBall;
    }

    public void TestReleaseBall()
    {
        if (controlledBall != null)
        {
            controlledBall.SetState(BallState.Free);
        }
    }

}