using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IInteractable
    {
        public void OnInteract(PlayerMovement playerMovement);
        public bool CanInteract { get; }
        public string InteractionName { get; }
        public bool CanHold { get; }
    }
}