using System;
using Assets.Scripts;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour, IInteractable
{
    [SerializeField] protected int _price;
    protected PlayerMovement _playerMovement;

    public void OnInteract(PlayerMovement playerMovement)
    {
        if (playerMovement.Coins < _price) return;
        DoUpgrade(playerMovement);
        playerMovement.Coins -= _price;


    }

    private void Start()
    {
        _playerMovement =  GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();;
    }

    protected abstract void DoUpgrade(PlayerMovement playerMovement);
    protected abstract string GetName();
    protected abstract string GetSecondName();
    public bool CanInteract => true;
    public string InteractionName => GetName();
    public string InteractionDescription => GetSecondName();
}