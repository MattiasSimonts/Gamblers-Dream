using UnityEngine;

public class PileOfCoins : Upgrade
{
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        
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
        return $"";
    }
}
