using UnityEngine;

public interface IHero
{
    Transform GetDribbleOrigin();
    string GetCharacterName();
    Team GetTeam();
    abstract void SetControlledBall(BallController theBall);
}
