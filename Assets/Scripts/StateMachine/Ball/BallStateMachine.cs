using UnityEngine;

public class BallStateMachine : StateMachine
{
    private BallController ballController;
    private BallIdleState ballIdleState;
    private BallFreeState ballFreeState;
    private BallTakenState ballTakenState;
    private BallFlyToRingState ballFlyToRingState;

    public BallController BallController => ballController;

    [Header("Debug Info")]
    [SerializeField] private string currentStateName;

    void Awake()
    {
        ballController = GetComponent<BallController>();
        InitializeStates();
    }

    void Start()
    {
        SwitchState(ballIdleState);
        ConnectToEvents();
    }

    protected override void Update()
    {
        base.Update();
        currentStateName = CurrentState?.GetType().Name ?? "None";
    }

    private void InitializeStates()
    {
        ballIdleState = new BallIdleState(this);
        ballFreeState = new BallFreeState(this);
        ballTakenState = new BallTakenState(this);
        ballFlyToRingState = new BallFlyToRingState(this);
    }

    public void TransitionToIdle()
    {
        SwitchState(ballIdleState);
    }

    public void TransitionToFree()
    {
        SwitchState(ballFreeState);
    }

    public void TransitionToTaken()
    {
        SwitchState(ballTakenState);
    }

    public void TransitionToFlyToRing()
    {
        SwitchState(ballFlyToRingState);
    }

    private void ConnectToEvents()
    {
        
    }

    void OnDestroy()
    {
        
    }
}
