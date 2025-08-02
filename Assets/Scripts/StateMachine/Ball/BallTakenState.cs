using UnityEngine;

public class BallTakenState : State
{
    private BallStateMachine stateMachine;

    public BallTakenState(BallStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        
        var ball = stateMachine.BallController;
        
        ball.BallRigidbody.isKinematic = true;
        ball.GetComponent<SphereCollider>().isTrigger = true;
        ball.BallRigidbody.useGravity = false;
    }

    public override void Tick(float deltaTime)
    {
        var ball = stateMachine.BallController;
        var handler = ball.GetCurrentBallHandler();
        
        if (handler == null)
        {
            stateMachine.TransitionToFree();
            return;
        }

        ball.UpdateBouncing(deltaTime);
    }

    public override void Exit()
    {
        
    }
}
