using UnityEngine;

public class PassingState : State
{
    private PlayerStateMachine stateMachine;
    private float passingTimer;
    private bool hasExecutedPass;

    public PassingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        passingTimer = 0f;
        hasExecutedPass = false;
        
        ExecutePass();
        hasExecutedPass = true;
    }

    public override void Tick(float deltaTime)
    {
        passingTimer += deltaTime;
        
        if (passingTimer >= 0.3f)
        {
            stateMachine.OnBallReleased();
        }
    }

    public override void Exit()
    {
        
    }

    private void ExecutePass()
    {
        var playerController = stateMachine.PlayerController;
        var controlledBall = playerController.ControlledBall;
        
        if (controlledBall != null)
        {
            controlledBall.BallStateMachine.TransitionToFree();
            controlledBall.transform.SetParent(null);
            controlledBall.ClearBallHandler();
            controlledBall.SetPickupCooldown();
            
            Vector3 passDirection = (playerController.transform.forward * playerController.PassingPower) + 
                                  (playerController.transform.up * playerController.PassingPower / 4);
            controlledBall.BallRigidbody.AddForce(passDirection, ForceMode.Impulse);
            
            GameEvents.TriggerBallPassed(playerController, passDirection);
        }
    }
}
