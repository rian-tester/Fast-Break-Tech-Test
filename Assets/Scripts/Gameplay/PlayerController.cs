using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    [SerializeField]
    private float passingPower = 30f;
    [SerializeField]
    float playerAccuracy = 80f;

    [SerializeField]
    public PlayerStateMachine playerStateMachine;

    public float PassingPower => passingPower;
    public float PlayerAccuracy => playerAccuracy;
    public BallController ControlledBall => controlledBall;

    protected override void Awake()
    {
        base.Awake();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        if (playerStateMachine == null)
            playerStateMachine = gameObject.AddComponent<PlayerStateMachine>();
    }

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
        if (context.performed)
        {
            playerStateMachine.HandlePassInput();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerStateMachine.HandleShootInput();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerStateMachine.TestPickupBall();
        }
    }



    public override void SetControlledBall(BallController theBall)
    {
        if (theBall == null)
        {
            controlledBall = null;
            return;
        }

        if (playerStateMachine.CurrentState is EmptyHandedState)
        {
            controlledBall = theBall;
            playerStateMachine.OnBallPickedUp();
            GameEvents.TriggerBallPickedUp(this, theBall);
        }
        else
        {
            Debug.LogWarning($"{GetCharacterName()} cannot take ball - not in empty handed state (current state: {playerStateMachine.CurrentState.GetType().Name})");
        }
    }
    public void TestReleaseBall()
    {
        if (controlledBall != null)
        {
            controlledBall.BallStateMachine.TransitionToFree();
            controlledBall.transform.SetParent(null);
            controlledBall.BallRigidbody.WakeUp();
            controlledBall.BallRigidbody.AddForce(transform.forward * passingPower/10);
            controlledBall = null;
            playerStateMachine.OnBallReleased();
        }
    }

    public void TestStateMachine()
    {
        Debug.Log($"=== Testing State Machine for {name} ===");
        Debug.Log($"Current state: {playerStateMachine.CurrentState.GetType().Name}");
        
        playerStateMachine.TestPickupBall();
        
        Debug.Log("Testing pass input...");
        playerStateMachine.HandlePassInput();
        
        Debug.Log("Testing shoot input...");
        playerStateMachine.HandleShootInput();
    }

    [ContextMenu("Test State Transitions")]
    public void TestStateTransitions()
    {
        if (!Application.isPlaying) return;
        
        Debug.Log("=== Manual State Testing ===");
        Debug.Log($"Current State: {playerStateMachine.CurrentState.GetType().Name}");
        
        if (playerStateMachine.CurrentState is EmptyHandedState)
        {
            Debug.Log("Simulating ball pickup...");
            playerStateMachine.OnBallPickedUp();
        }
        else if (playerStateMachine.CurrentState is DribblingState)
        {
            Debug.Log("In dribbling state - can pass or shoot");
        }
    }

    [ContextMenu("Force Empty Handed")]
    public void ForceEmptyHanded()
    {
        if (!Application.isPlaying) return;
        playerStateMachine.OnBallReleased();
    }

    [ContextMenu("Force Pickup Ball")]
    public void ForcePickupBall()
    {
        if (!Application.isPlaying) return;
        playerStateMachine.OnBallPickedUp();
    }
}