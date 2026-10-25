using System.Collections;
using UnityEngine;

public class GamblingChip : Upgrade
{
    protected override void DoUpgrade(PlayerMovement playerMovement)
    {
        playerMovement.GamblingChipUpgrades++;
    }

    protected override string GetDescription()
    {
        return "";
    }

    protected override string GetName()
    {
        return "";
    }

    protected override string GetSecondName()
    {
        return "";
    }

    protected override string GetTitle()
    {
        return "Increases the amount of active machines";
    }
}
