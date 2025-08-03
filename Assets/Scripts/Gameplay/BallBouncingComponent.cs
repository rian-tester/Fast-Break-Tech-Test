using UnityEngine;

public class BallBouncingComponent : MonoBehaviour, IBallComponent
{
    // this class is required as component by BallController
    // this class related to taken ballstate and hero dribbling state
    // it simulate lerping to imitate bouncing
    // it need dribble anchor from player
    // when bouncing it should be in kinematic
    // also responsible to make it unbounce
    // use existing system as helper, statemachine
    // and how about it to make it idle again?
    public void Initialize(BallController controller)
    {
        throw new System.NotImplementedException();
    }

    public void OnStateChanged(State newState)
    {
        throw new System.NotImplementedException();
    }
}
