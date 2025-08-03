using UnityEngine;

public class AIController : HeroBase
{
    // Will automatically inherit:
    // - Movement capabilities
    // - Shooting parameters
    // - Passing power
    // - Team settings
    // - All component references

    // Only needs to implement:
    // - AI decision making
    // - AI state machine
    // - No input handling needed!
    public override void SetControlledBall(BallController theBall)
    {
        throw new System.NotImplementedException();
    }
}
