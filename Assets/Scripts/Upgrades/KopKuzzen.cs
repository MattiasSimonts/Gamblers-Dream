using UnityEngine;

public class KopKuzzen : Upgrade
{
    private static int _amountOfPurchases = 0;
    
    [SerializeField] private float _reduceDamageTaken = 0.1f;
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        _playerMovement.DamageOvertime -= _reduceDamageTaken;
        _amountOfPurchases++;
    }

    protected override string GetName()
    {
        return $"Pillow: {_price} coins.";
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {_amountOfPurchases} pillow upgrades";
    }

    protected override string GetTitle()
    {
        return "Pillow";
    }

    protected override string GetDescription()
    {
        return "Decreases the damage taken over time";
    }
    
}
