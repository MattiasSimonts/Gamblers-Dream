using UnityEngine;

public class PileOfCoins : Upgrade
{
    private static int _amountOfPurchases = 0;
    
    [SerializeField] private int _extraCoinsAmount;
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        _playerMovement.StartingCoins += _extraCoinsAmount;
        _amountOfPurchases++;
    }

    protected override string GetName()
    {
        return $"Pile of Coins: {_price} Coins";
    }

    protected override string GetSecondName()
    {
       return $"Currently you have {_amountOfPurchases} pile of coins upgrades";
    }

    protected override string GetTitle()
    {
        return "Pile of Coins";
    }

    protected override string GetDescription()
    {
        return $"Increases starting money by {_extraCoinsAmount}";
    }
}
