using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField]
    private Transform playerDribbleAnchor;

    public Transform GetDribbleOrigin()
    {
        return playerDribbleAnchor;
    }
}