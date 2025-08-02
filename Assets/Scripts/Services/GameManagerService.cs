using UnityEngine;

public class GameManager : IService
{
    private BasketballRing[] basketballRings;

    public string GetServiceName()
    {
        return "GameManager";
    }

    public void Initialize()
    {
        basketballRings = Object.FindObjectsByType<BasketballRing>(FindObjectsSortMode.None);
        Debug.Log($"GameManager initialized with {basketballRings.Length} basketball rings");
    }

    public void Shutdown()
    {
        basketballRings = null;
    }

    public BasketballRing GetTargetRing(Team playerTeam)
    {
        foreach (var ring in basketballRings)
        {
            if (ring.DefendingTeam != playerTeam)
            {
                return ring;
            }
        }
        return null;
    }

    public BasketballRing[] GetAllRings()
    {
        return basketballRings;
    }
}