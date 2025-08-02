using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    [SerializeField]
    private float passingPower;
    [SerializeField]
    private float shootingPower;
    [SerializeField]
    private float shootingArc;

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
            SetState(CharacterState.Passing);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && characterState == CharacterState.Dribbling) 
            SetState(CharacterState.Shooting);
    }

    protected override void OnStateChanged(CharacterState newState, CharacterState oldState)
    {
        switch (newState)
        {
            case CharacterState.EmptyHanded:
                if (controlledBall != null)
                {
                    controlledBall = null;
                }
                break;

            case CharacterState.Passing:
                if (controlledBall == null) return;
                controlledBall.SetState(BallState.Free);
                controlledBall.transform.SetParent(null);
                controlledBall.BallRigidbody.WakeUp();
                controlledBall.BallRigidbody.AddForce((transform.forward * passingPower) + (transform.up * passingPower/4), ForceMode.Acceleration); 
                controlledBall = null;
                SetState(CharacterState.EmptyHanded);

                break;

            case CharacterState.Shooting:
                if (controlledBall == null) return;
                
                GameManager gameManager = ServiceLocator.Get<GameManager>();
                if (gameManager != null)
                {
                    BasketballRing targetRing = gameManager.GetTargetRing(playerTeam);
                    if (targetRing != null)
                    {
                        ShootBall(targetRing);
                        Debug.Log($"{GetCharacterName()} shoots toward {targetRing.DefendingTeam} ring!");
                    }
                }
                break;

            default:
                break;
        }
    }
    public void TestReleaseBall()
    {
        if (controlledBall != null)
        {
            controlledBall.SetState(BallState.Free);
            controlledBall.transform.SetParent(null);
            controlledBall.BallRigidbody.WakeUp();
            controlledBall.BallRigidbody.AddForce(transform.forward * passingPower/10);
            controlledBall = null;
            SetState(CharacterState.EmptyHanded);
        }
    }

    private void ShootBall(BasketballRing targetRing)
    {
        controlledBall.SetState(BallState.Free);
        controlledBall.transform.SetParent(null);
        controlledBall.BallRigidbody.WakeUp();

        Vector3 targetPosition = targetRing.ShootingTarget.position;
        Vector3 shootDirection = (targetPosition - transform.position).normalized;
        Vector3 shootForce = (shootDirection * shootingPower) + (Vector3.up * shootingArc);

        controlledBall.BallRigidbody.AddForce(shootForce, ForceMode.Acceleration);
        controlledBall = null;
        SetState(CharacterState.EmptyHanded);
    }
}