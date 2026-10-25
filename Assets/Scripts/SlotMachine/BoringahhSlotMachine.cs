using Assets.Scripts;
using EasyTransition;
using UnityEngine;

public class BoringahhSlotMachine : MonoBehaviour, IInteractable
{
    [SerializeField] private TransitionSettings _transition;


    public void OnInteract(PlayerMovement playerMovement)
    {
            TransitionManager.Instance().Transition("OutdoorsScene", _transition, 0);
    }

    public bool CanInteract => true;
    public string InteractionName => "Start Game";
    public string InteractionDescription => "";
}