public interface IBallComponent
{
    void Initialize(BallController controller);
    void OnStateChanged(State newState);
}
