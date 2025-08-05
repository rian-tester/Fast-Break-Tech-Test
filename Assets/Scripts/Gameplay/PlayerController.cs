using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    [SerializeField]
    private float passingPower;
    [SerializeField]
    float playerAccuracy = 100f;

    protected override void Update()
    {
        base.Update();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void OnPass(InputAction.CallbackContext context)
    {
        if (context.performed && characterState == CharacterState.Dribbling)
            SetCharacterState(CharacterState.Passing);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && characterState == CharacterState.Dribbling)
            SetCharacterState(CharacterState.Shooting);
    }

    protected override void OnStateChanged(CharacterState newState, CharacterState oldState)
    {
        switch (newState)
        {
            case CharacterState.EmptyHanded:
                
                if (controlledBall != null)
                {
                    controlledBall.transform.SetParent(null);
                    controlledBall = null;

                }
                break;

            case CharacterState.Dribbling:
                if (controlledBall == null) return;
                controlledBall.SetBallState(BallState.Taken);


                break;

            case CharacterState.Passing:
                if (controlledBall == null) return;
                controlledBall.SetBallState(BallState.Free);
                controlledBall.transform.SetParent(null);
                controlledBall.BallRigidbody.WakeUp();
                controlledBall.BallRigidbody.AddForce((transform.forward * passingPower) + (transform.up * passingPower/4), ForceMode.Acceleration); 
                controlledBall = null;
                SetCharacterState(CharacterState.EmptyHanded);

                break;

            case CharacterState.Shooting:
                if (controlledBall == null) return;
                
                GameManager gameManager = ServiceLocator.Get<GameManager>();
                if (gameManager != null)
                {
                    BasketballRing targetRing = gameManager.GetTargetRing(playerTeam);
                    if (targetRing != null)
                    {
                        controlledBall.SetupShootingTarget(targetRing.ShootingTarget.position, playerAccuracy);
                        controlledBall.SetBallState(BallState.FlyToRing);
                        controlledBall.transform.SetParent(null);
                        controlledBall = null;
                        
                        Debug.Log($"{GetCharacterName()} shoots toward {targetRing.DefendingTeam} ring!");
                    }
                }
                SetCharacterState(CharacterState.EmptyHanded);
                break;

            default:
                break;
        }
    }
    public void TestReleaseBall()
    {
        if (controlledBall != null)
        {
            controlledBall.SetBallState(BallState.Free);
            controlledBall.transform.SetParent(null);
            controlledBall.BallRigidbody.WakeUp();
            controlledBall.BallRigidbody.AddForce(transform.forward * passingPower/10);
            controlledBall = null;
            SetCharacterState(CharacterState.EmptyHanded);
        }
    }
}
