using UnityEngine;

public class BallFlyToRingState : State
{
    private BallStateMachine stateMachine;

    public BallFlyToRingState(BallStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        
        var ball = stateMachine.BallController;
        ball.BallRigidbody.isKinematic = true;
        ball.GetComponent<SphereCollider>().isTrigger = true;
        ball.BallRigidbody.mass = 0f;
        ball.BallRigidbody.useGravity = false;
        
        if (ball.transform.parent != null) 
            ball.transform.SetParent(null);
        
        ball.ClearBallHandler();
        ball.BallRigidbody.AddTorque(ball.GiveRandomFloat(), ball.GiveRandomFloat(), ball.GiveRandomFloat());
        ball.StartFlightCoroutine();
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Ball exited FlyToRing state");
    }

    public void OnFlightComplete()
    {
        stateMachine.TransitionToFree();
    }
}
