using UnityEngine;
public interface ICharacter
{
    Transform GetDribbleOrigin();
    void SetControlledBall(BallController theBall);
}
