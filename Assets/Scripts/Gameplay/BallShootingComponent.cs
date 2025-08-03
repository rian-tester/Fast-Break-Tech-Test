using UnityEngine;
using System.Collections;

public class BallShootingComponent : MonoBehaviour, IBallComponent
{
    // this class is required as component by BallController
    // this will hold variable for shooting mechanic need by old logic
    // this class will also execute shooting fly mechanic
    // however it needs physic condition done by BallPhysics component before it start
    // And also need information from state machine what state are now this can only happen during FlyToRing
    // this class also responsible to handle post flying logic back to Free state and all related physics   
    public void Initialize(BallController controller)
    {
        throw new System.NotImplementedException();
    }

    public void OnStateChanged(State newState)
    {
        throw new System.NotImplementedException();
    }
}
