using Assets.Scripts;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InteractableCoin : MonoBehaviour, IInteractable
{
    public bool CanInteract { get { return true; } }

    public void OnInteract(PlayerMovement playerMovement)
    {
        playerMovement.AddCoins(1);
        Destroy(gameObject);
    }
}
