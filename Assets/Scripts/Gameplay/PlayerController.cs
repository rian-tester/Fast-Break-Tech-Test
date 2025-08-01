using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    void Start()
    {
        controlledBall = null;
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
        }
    }
}