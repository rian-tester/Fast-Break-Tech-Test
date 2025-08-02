using UnityEngine;

public class BasketballRing : MonoBehaviour
{
    [SerializeField]
    private Team defendingTeam;

    [SerializeField]
    private Transform shootingTarget;

    public Team DefendingTeam => defendingTeam;
    public Transform ShootingTarget => shootingTarget;
}
