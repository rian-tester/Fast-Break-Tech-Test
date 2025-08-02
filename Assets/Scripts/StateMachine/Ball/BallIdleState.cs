using UnityEngine;

public class BallIdleState : State
{
    private BallStateMachine stateMachine;

    public BallIdleState(BallStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Ball entered Idle state");
        GameEvents.TriggerBallStateChanged(stateMachine.BallController, BallState.Idle, BallState.Idle);
        
        var ball = stateMachine.BallController;
        ball.BallRigidbody.isKinematic = false;
        ball.GetComponent<SphereCollider>().isTrigger = false;
        ball.BallRigidbody.mass = 1f;
        ball.BallRigidbody.useGravity = true;
        
        if (ball.transform.parent != null) 
            ball.transform.SetParent(null);
        
        ball.BallRigidbody.WakeUp();
        ball.ClearBallHandler();
    }

    public override void Tick(float deltaTime)
    {
        var ball = stateMachine.BallController;
        if (ball.BallRigidbody.linearVelocity.magnitude > Mathf.Epsilon || 
            ball.BallRigidbody.angularVelocity.magnitude > Mathf.Epsilon)
        {
            stateMachine.TransitionToFree();
        }
    }

    public override void Exit()
    {
        Debug.Log("Ball exited Idle state");
    }
}
