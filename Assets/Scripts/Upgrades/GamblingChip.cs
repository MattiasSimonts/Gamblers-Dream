using System.Collections;
using UnityEngine;

public class GamblingChip : Upgrade
{
    private static int _amountOfPurchases = 0;
    
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        playerMovement.GamblingChipUpgrades++;
        _amountOfPurchases++;
    }

    protected override string GetDescription()
    {
        return "Increases the amount of active machines";
    }

    protected override string GetName()
    {
        return $"GamblingChip: {_price} coins";;
    }

    protected override string GetSecondName()
    {
        return $"Currently you have {2+_amountOfPurchases} Active Gambling machines";   
    }

    protected override string GetTitle()
    {
        return "Increases the amount of active machines";
    }
}
