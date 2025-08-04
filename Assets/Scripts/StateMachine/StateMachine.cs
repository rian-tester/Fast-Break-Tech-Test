using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;

    public State CurrentState => currentState;

    protected virtual void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public bool IsInState<T>() where T : State
    {
        return currentState is T;
    }
}
