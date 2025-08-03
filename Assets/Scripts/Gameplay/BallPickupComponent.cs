using UnityEngine;

public class BallPickupComponent : MonoBehaviour, IBallComponent
{
    // this class is required as component by BallController
    // This class will need trigger enter to execute pickup
    // this class will also handle reference passing i think old logic has it
    // should we use existing game events?
    // Also need to handle physics which done by Physicscomponent
    // Also communicate or feed state from state machine
    public void Initialize(BallController controller)
    {
        throw new System.NotImplementedException();
    }

    public void OnStateChanged(State newState)
    {
        throw new System.NotImplementedException();
    }
}
