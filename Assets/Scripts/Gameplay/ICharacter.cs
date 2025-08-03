using UnityEngine;

public interface ICharacter
{
    Transform GetDribbleOrigin();
    string GetCharacterName();
    Team GetTeam();
    abstract void SetControlledBall(BallController theBall);
}
