using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;

public class InteractableCoin : MonoBehaviour, IInteractable
{
    public bool CanInteract => true;

    [SerializeField] private float _pickUpRadius = 500;

    public string InteractionName => "Left click to pickup coin";

    public void OnInteract(PlayerMovement playerMovement)
    {
        HashSet<InteractableCoin> pickedUpCoins = new HashSet<InteractableCoin> { this };
        for (int i = 0; i < playerMovement.CoinPickupUpgrades; i++)
        {
            foreach (InteractableCoin interactableCoin in pickedUpCoins.ToList().SelectMany(
                         coin => coin.GetTouchingCoins(), 
                         (coin, touchingCoin) => touchingCoin.GetComponent<InteractableCoin>()).Where(interactableCoin => interactableCoin))
                pickedUpCoins.Add(interactableCoin);
            
        }
        
        playerMovement.AddCoins(pickedUpCoins.Count);
        foreach (InteractableCoin interactableCoin in pickedUpCoins)
        {
            Destroy(interactableCoin.gameObject);
        }
    }
    private readonly Collider[] _possibleCoins = new Collider[64];

    private List<GameObject> GetTouchingCoins()
    {
        var coins = new List<GameObject>();
        int count = Physics.OverlapSphereNonAlloc(transform.position, _pickUpRadius, _possibleCoins);

        for (int i = 0; i < count; i++)
        {
            if (_possibleCoins[i].CompareTag("Coin"))
                coins.Add(_possibleCoins[i].gameObject);
        }

        return coins;
    }
}