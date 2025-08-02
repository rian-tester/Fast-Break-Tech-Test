using UnityEngine;

public class DribblingState : State
{
    private PlayerStateMachine stateMachine;

    public DribblingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        
        var playerController = stateMachine.PlayerController;
        var controlledBall = playerController.ControlledBall;
        
        if (controlledBall != null)
        {
            controlledBall.BallStateMachine.TransitionToTaken();
        }
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        Debug.Log($"{stateMachine.PlayerController.name} exited Dribbling state");
    }
}
