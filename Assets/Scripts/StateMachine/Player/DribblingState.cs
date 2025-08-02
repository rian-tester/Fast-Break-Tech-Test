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
        Debug.Log($"{stateMachine.PlayerController.name} entered Dribbling state");
        GameEvents.TriggerPlayerStateChanged(stateMachine.PlayerController, CharacterState.Dribbling, CharacterState.EmptyHanded);
        
        var playerController = stateMachine.PlayerController;
        var controlledBall = playerController.ControlledBall;
        
        if (controlledBall != null)
        {
            controlledBall.SetBallState(BallState.Taken);
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
