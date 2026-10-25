using Assets.Scripts;
using UnityEngine;

public class PullLever : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _slotMachineParent;
    private SlotMachine _slotMachine;

    private void Awake()
    {
        _slotMachine = _slotMachineParent.GetComponent<SlotMachine>();
    }

    public void OnInteract(PlayerMovement playerMovement)
    {
        _slotMachine.Spin();
    }

    public bool CanInteract => !_slotMachine.IsSpinning && _slotMachine.HasCoins;

    public string InteractionName => "Left click to gamble";

    public bool CanHold => false;
}
