using System;
using Assets.Scripts;
using UnityEngine;

public class InsertCoin : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _slotMachineParent;
    private SlotMachine _slotMachine;

    private void Awake()
    {
        _slotMachine = _slotMachineParent.GetComponent<SlotMachine>();
    }

    public void OnInteract()
    {
        _slotMachine.InsertCoin();
    }
}
