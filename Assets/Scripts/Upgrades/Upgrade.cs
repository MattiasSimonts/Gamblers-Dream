using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Upgrade : MonoBehaviour, IInteractable
{
    [SerializeField] protected int _price;
    protected int _amountOfPurchases = 0;
    private bool HasPurchast = false;
    protected PlayerMovement _playerMovement;

    public void OnInteract(PlayerMovement playerMovement)
    {
        if (playerMovement.Coins < _price) return;
        DoUpgrade(playerMovement);
        playerMovement.Coins -= _price;
        HasPurchast = true;
        _amountOfPurchases++;


    }

    private void Start()
    {
        _playerMovement =  GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();;
    }

    protected abstract void DoUpgrade(PlayerMovement playerMovement);
    protected abstract string GetName();
    protected abstract string GetSecondName();
    public bool CanInteract => true;
    public string InteractionName => HasPurchast ? "You have bought the maximum of this upgrade." : GetName();
    public string InteractionDescription => HasPurchast ? "Come By an other time!" : GetName();
    
    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HasPurchast = false;
    }
}