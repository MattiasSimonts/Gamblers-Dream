using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;

public class InteractableCoin : MonoBehaviour, IInteractable
{
    public bool CanInteract => true;

    [SerializeField] private float _pickUpRadius = 0.0f;

    private static readonly List<InteractableCoin> _allCoins = new List<InteractableCoin>();
    private void OnEnable() => _allCoins.Add(this);
    private void OnDisable() => _allCoins.Remove(this);

    public string InteractionName => "Left click to pickup coin";

    public void OnInteract(PlayerMovement playerMovement)
    {
        HashSet<InteractableCoin> pickedUpCoins = new HashSet<InteractableCoin> { this };
        if (playerMovement.CoinPickupUpgrades != 0)
        {
            for (int i = 0; i < playerMovement.CoinPickupUpgrades; i++)
            {
                foreach (InteractableCoin interactableCoin in pickedUpCoins.ToList())
                {
                    var test = interactableCoin.GetTouchingCoins().ToList();
                    foreach (InteractableCoin touchingCoin in test)
                    {
                        pickedUpCoins.Add(touchingCoin);
                    }
                }
            }
        }

        playerMovement.AddCoins(pickedUpCoins.Count);
        foreach (InteractableCoin interactableCoin in pickedUpCoins)
        {
            interactableCoin.Kill();
        }
    }


    private List<InteractableCoin> GetTouchingCoins()
    {
        foreach (var coin in _allCoins)
            Debug.Log($"{coin.name}: dist {Vector3.Distance(coin.transform.position, transform.position)}, radius {_pickUpRadius}", coin);
        return _allCoins.Where(coin => Vector3.Distance(coin.transform.position, transform.position) <= _pickUpRadius).ToList();
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}