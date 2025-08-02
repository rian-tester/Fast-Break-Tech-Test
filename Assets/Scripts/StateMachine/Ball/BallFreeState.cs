using UnityEngine;

public class BallFreeState : State
{
    private BallStateMachine stateMachine;

    public BallFreeState(BallStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        
        var ball = stateMachine.BallController;
        ball.BallRigidbody.isKinematic = false;
        ball.GetComponent<SphereCollider>().isTrigger = false;
        ball.BallRigidbody.mass = 1f;
        ball.BallRigidbody.useGravity = true;
        
        if (ball.transform.parent != null) 
            ball.transform.SetParent(null);
        
        ball.BallRigidbody.WakeUp();
        ball.ClearBallHandler();
        ball.BallRigidbody.AddTorque(ball.GiveRandomFloat(), ball.GiveRandomFloat(), ball.GiveRandomFloat());
    }

    public override void Tick(float deltaTime)
    {
        var ball = stateMachine.BallController;
        if (ball.BallRigidbody.linearVelocity.magnitude < Mathf.Epsilon && 
            ball.BallRigidbody.angularVelocity.magnitude < Mathf.Epsilon)
        {
            stateMachine.TransitionToIdle();
        }
    }

    public override void Exit()
    {
        Debug.Log("Ball exited Free state");
    }
}
