using UnityEngine;

public class ShootingState : State
{
    private PlayerStateMachine stateMachine;
    private float shootingTimer;
    private bool hasExecutedShoot;

    public ShootingState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        shootingTimer = 0f;
        hasExecutedShoot = false;
    }

    public override void Tick(float deltaTime)
    {
        shootingTimer += deltaTime;
        
        if (!hasExecutedShoot && shootingTimer >= 0.1f)
        {
            ExecuteShoot();
            hasExecutedShoot = true;
        }
        
        if (shootingTimer >= 0.8f)
        {
            stateMachine.OnBallReleased();
        }
    }

    public override void Exit()
    {
        
    }

    private void ExecuteShoot()
    {
        var playerController = stateMachine.PlayerController;
        var controlledBall = playerController.ControlledBall;
        
        if (controlledBall != null)
        {
            GameManager gameManager = ServiceLocator.Get<GameManager>();
            if (gameManager != null)
            {
                BasketballRing targetRing = gameManager.GetTargetRing(playerController.GetTeam());
                if (targetRing != null)
                {
                    controlledBall.SetupShootingTarget(
                        targetRing.ShootingTarget.position, 
                        playerController.PlayerAccuracy,
                        playerController.FlightTimeMultiplier,
                        playerController.ArcHeightMultiplier
                    );
                    controlledBall.BallStateMachine.TransitionToFlyToRing();
                    controlledBall.transform.SetParent(null);
                    controlledBall.ClearBallHandler();
                    controlledBall.SetPickupCooldown();
                    
                    GameEvents.TriggerBallShot(playerController, targetRing);
                }
            }
        }
    }
}
