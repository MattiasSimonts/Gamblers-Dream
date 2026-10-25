using System.Collections;
using UnityEngine;

public class Blikje : Upgrade
{
    [SerializeField] private float _amountExtraSpeed = 0.005f;
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        _playerMovement.SprintSpeed += _amountExtraSpeed;
    }

    protected override string GetDescription()
    {
        return $"Slightly Increases sprinting speed";
    }

    protected override string GetName()
    {
        return $"Can: {_price} coins";
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {_playerMovement.SprintSpeed} sprint speed";
    }

    protected override string GetTitle()
    {
        return $"Can";
    }
}