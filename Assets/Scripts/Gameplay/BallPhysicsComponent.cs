using UnityEngine;

public class BallPhysicsComponent : MonoBehaviour, IBallComponent
{
    // this class is required as component by BallController
    // this class mainly executor for all physics condition
    // just to receive signals and do physics condition based on receive instructio
    // maybe to fire signal if it is done with its task
    public void Initialize(BallController controller)
    {
        throw new System.NotImplementedException();
    }

    public void OnStateChanged(State newState)
    {
        throw new System.NotImplementedException();
    }
}
