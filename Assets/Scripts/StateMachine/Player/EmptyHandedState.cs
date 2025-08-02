using UnityEngine;

public class EmptyHandedState : State
{
    private PlayerStateMachine stateMachine;

    public EmptyHandedState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        Debug.Log($"{stateMachine.PlayerController.name} entered EmptyHanded state");
        GameEvents.TriggerPlayerStateChanged(stateMachine.PlayerController, CharacterState.EmptyHanded, CharacterState.EmptyHanded);
        
        var playerController = stateMachine.PlayerController;
        var controlledBall = playerController.ControlledBall;
        
        if (controlledBall != null)
        {
            controlledBall.transform.SetParent(null);
            playerController.SetControlledBall(null);
        }
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        Debug.Log($"{stateMachine.PlayerController.name} exited EmptyHanded state");
    }
}
