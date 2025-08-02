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
    [SerializeField] private string currentStateName;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        InitializeStates();
    }

    void Start()
    {
        SwitchState(emptyHandedState);
        ConnectToBallEvents();
    }

    protected override void Update()
    {
        base.Update();
        currentStateName = CurrentState?.GetType().Name ?? "None";
    }

    private void InitializeStates()
    {
        emptyHandedState = new EmptyHandedState(this);
        dribblingState = new DribblingState(this);
        passingState = new PassingState(this);
        shootingState = new ShootingState(this);
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

    public void TestPickupBall()
    {
        Debug.Log($"Testing ball pickup for {playerController.name}");
        OnBallPickedUp();
    }

    public void ConnectToBallEvents()
    {
        GameEvents.OnBallReleased += HandleBallReleased;
    }

    public void DisconnectFromBallEvents()
    {
        GameEvents.OnBallReleased -= HandleBallReleased;
    }

    private void HandleBallReleased(BallController ball)
    {
        if (ball == playerController.ControlledBall)
        {
            OnBallReleased();
        }
    }

    void OnDestroy()
    {
        DisconnectFromBallEvents();
    }
}
