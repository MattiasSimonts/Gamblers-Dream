using UnityEngine;

public class EnergyBar : Upgrade
{
    private static int _amountOfPurchases = 0;
    
    [SerializeField] private float _extraMoveSpeed = 1f;
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        playerMovement.MovementSpeed += _extraMoveSpeed;
        _amountOfPurchases++;
    }   

    protected override string GetName()
    {
        return $"Energy Bar: {_price} coins";
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {_amountOfPurchases} Speed upgrades";
    }

    protected override string GetTitle()
    {
        return "Energy Bar";
    }

    protected override string GetDescription()
    {
        return "Slightly increases move speed";
    }
}
