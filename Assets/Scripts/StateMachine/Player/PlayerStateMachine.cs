using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    private PlayerController playerController;
    private EmptyHandedState emptyHandedState;
    private DribblingState dribblingState;
    private PassingState passingState;
    private ShootingState shootingState;

    public PlayerController PlayerController => playerController;

    [Header("Debug Info")]
    [SerializeField, ReadOnly] private string currentStateName;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        InitializeStates();
    }

    void Start()
    {
        SwitchState(emptyHandedState);
        SubscribeToBallEvents();
    }

    void OnDestroy()
    {
        UnsubscribeFromBallEvents();
    }

     public void SubscribeToBallEvents()
    {
        GameEvents.OnBallReleased += HandleBallReleased;
    }

    public void UnsubscribeFromBallEvents()
    {
        GameEvents.OnBallReleased -= HandleBallReleased;
    }


    private void InitializeStates()
    {
        emptyHandedState = new EmptyHandedState(this);
        dribblingState = new DribblingState(this);
        passingState = new PassingState(this);
        shootingState = new ShootingState(this);
    }

    public override void SwitchState(State newState)
    {
        base.SwitchState(newState);
        currentStateName = CurrentState.GetType().Name;
    }
    private void HandleBallReleased(BallController ball)
    {
        if (ball == playerController.ControlledBall)
        {
            OnBallReleased();
        }
    }
    public void HandlePassInput()
    {
        if (CurrentState is DribblingState)
        {
            SwitchState(passingState);
        }
    }

    public void HandleShootInput()
    {
        if (CurrentState is DribblingState)
        {
            SwitchState(shootingState);
        }
    }

    public void OnBallPickedUp()
    {
        if (CurrentState is EmptyHandedState)
        {
            SwitchState(dribblingState);
        }
    }

    public void OnBallReleased()
    {
        playerController.SetControlledBall(null);
        SwitchState(emptyHandedState);
    }
}
