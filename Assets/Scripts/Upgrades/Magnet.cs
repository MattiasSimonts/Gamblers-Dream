using Assets.Scripts;
using UnityEngine;

public class Magnet : Upgrade , IInteractable
{
    
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        playerMovement.CoinPickupUpgrades++;
    }

    protected override string GetName()
    {
        return $"Magnet: {_price} coins";
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {_playerMovement.CoinPickupUpgrades} magnet upgrades";
    }

    protected override string GetTitle()
    {
        return "Magnet";
    }

    protected override string GetDescription()
    {
        return "";
    }
}
