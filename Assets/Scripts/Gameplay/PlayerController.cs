using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{

    protected override void Update()
    {
        base.Update();
        Debug.Log(Velocity2D);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
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