using UnityEngine;

public class SleepingPils : Upgrade
{
    [SerializeField] private int _amountExtraHealth = 15;
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        _playerMovement.GetPlayerHp.MaxHealth += _amountExtraHealth;
    }

    protected override string GetName()
    {
        return $"Sleeping Pils: {_price} coins";
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {_amountOfPurchases} health upgrades";
    }

    protected override string GetTitle()
    {
        return "SleepingPils";
    }

    protected override string GetDescription()
    {
        return "";
    }
}
