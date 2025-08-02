using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<PlayerController, BallController> OnBallPickedUp;
    public static event Action<PlayerController, Vector3> OnBallPassed;
    public static event Action<PlayerController, BasketballRing> OnBallShot;
    public static event Action<BallController> OnBallReleased;
    public static event Action<PlayerController, CharacterState, CharacterState> OnPlayerStateChanged;
    public static event Action<BallController, BallState, BallState> OnBallStateChanged;

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

    public static void TriggerPlayerStateChanged(PlayerController player, CharacterState newState, CharacterState oldState)
    {
        OnPlayerStateChanged?.Invoke(player, newState, oldState);
    }

    public static void TriggerBallStateChanged(BallController ball, BallState newState, BallState oldState)
    {
        OnBallStateChanged?.Invoke(ball, newState, oldState);
    }
}
