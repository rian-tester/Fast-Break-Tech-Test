using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    [SerializeField]
    private float passingPower;

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
        if (context.performed) SetState(CharacterState.Passing);
    }

    protected override void OnStateChanged(CharacterState newState, CharacterState oldState)
    {
        switch (newState)
        {
            case CharacterState.EmptyHanded:

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
            controlledBall = null;
        }
    }
}