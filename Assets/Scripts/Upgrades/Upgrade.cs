using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Upgrade : MonoBehaviour, IInteractable
{
    [SerializeField] private PriceBoard _priceBoard;
    [SerializeField] protected int _price;
    private bool HasPurchast = false;
    protected PlayerMovement _playerMovement;

    public void OnInteract(PlayerMovement playerMovement)
    {
        if (playerMovement.ShopCoins < _price) return;
        DoUpgrade(playerMovement);
        playerMovement.ShopCoins -= _price;
        HasPurchast = true;


    }

    private void Start()
    {
        _playerMovement =  GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _priceBoard.SetText(GetTitle(), GetDescription());

    }

    protected abstract void DoUpgrade(PlayerMovement playerMovement);
    protected abstract string GetName();
    protected abstract string GetSecondName();
    protected abstract string GetTitle();
    protected abstract string GetDescription();
    public bool CanInteract => true;
    public string InteractionName => HasPurchast ? "You have bought the maximum of this upgrade." : GetName();
    public string InteractionDescription => HasPurchast ? "Come By an other time!" : GetSecondName();
    
    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HasPurchast = false;
    }
}