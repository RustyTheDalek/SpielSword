using UnityEngine;

/// <summary>
/// Basic Villager class for use in the Main menu village
/// Created by : Ian Jones - 09/04/18
/// </summary>
public class Basic : Villager
{
    public override void Awake()
    {
        base.Awake();

        VillageExit.OnPlayerLeftVillage += StopControl;
    }

    protected void StopControl()
    {
        xDir = 0;
        villagerState = VillagerState.Waiting;
    }
}
