using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<PlayerController, BallController> OnBallPickedUp;
    public static event Action<PlayerController, Vector3> OnBallPassed;
    public static event Action<PlayerController, BasketballRing> OnBallShot;
    public static event Action<BallController> OnBallReleased;

    public static void TriggerBallPickedUp(PlayerController player, BallController ball)
    {
        OnBallPickedUp?.Invoke(player, ball);
    }

    public static void TriggerBallPassed(PlayerController player, Vector3 direction)
    {
        OnBallPassed?.Invoke(player, direction);
    }

    public static void TriggerBallShot(PlayerController player, BasketballRing target)
    {
        OnBallShot?.Invoke(player, target);
    }

    public static void TriggerBallReleased(BallController ball)
    {
        OnBallReleased?.Invoke(ball);
    }
}
