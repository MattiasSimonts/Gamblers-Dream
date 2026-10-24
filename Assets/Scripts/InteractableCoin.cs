using Assets.Scripts;
using UnityEngine;

public class InteractableCoin : MonoBehaviour, IInteractable
{
    public void OnInteract(PlayerMovement playerMovement)
    {
        playerMovement.AddCoins(1);
        Destroy(gameObject);
    }
}
