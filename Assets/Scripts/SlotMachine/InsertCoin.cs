using System;
using Assets.Scripts;
using UnityEngine;

public class InsertCoin : MonoBehaviour, IInteractable
{
    [SerializeField] private float _startInteractCooldown = 0.5f;
    [SerializeField] private float _interactCooldownChange = 0.9f;
    private float _currentInteractCooldown;
    private float _currentInteractTimer;
    [SerializeField] GameObject _slotMachineParent;
    private SlotMachine _slotMachine;

    private void Awake()
    {
        _slotMachine = _slotMachineParent.GetComponent<SlotMachine>();
    }

    public void OnInteract(PlayerMovement playerMovement)
    {
        if (!_slotMachine.IsEnabled)
            return;
        TryInsertCoin(playerMovement);
    }

    private void TryInsertCoin(PlayerMovement playerMovement)
    {
        if (playerMovement.Coins < 1) return;
        _slotMachine.InsertCoin();
        playerMovement.AddCoins(-1);
    }

    public bool CanInteract => CanInteractBooleanMaker();

    public string InteractionName => _slotMachine.IsEnabled ? "Press left click to insert coin" : "Out of order";

    private bool CanInteractBooleanMaker()
    {
        return !_slotMachine.IsSpinning;
    }
}