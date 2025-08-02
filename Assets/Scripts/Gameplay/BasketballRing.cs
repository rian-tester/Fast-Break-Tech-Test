using UnityEngine;

public class BasketballRing : MonoBehaviour
{
    [SerializeField]
    private Team defendingTeam;

    [SerializeField]
    private Transform shootingTarget;

    public Team DefendingTeam => defendingTeam;
    public Transform ShootingTarget => shootingTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<BallController>();
            ball.SetState(BallState.Free);
        }
    }
}
