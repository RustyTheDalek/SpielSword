using UnityEngine;

/// <summary>
/// Basic Villager class for use in the Main menu village
/// Created by : Ian Jones - 09/04/18
/// Updated by : Ian Joens - 10/04/18
/// </summary>
public class Basic : Villager
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(VillageExit villageExit, VillageAndMapManager wMapManager)
    {
        villageExit.OnPlayerLeftVillage += StopControl;
        wMapManager.OnPlayerEnterVillage += StartControl;
    }

    protected void StopControl()
    {
        pData.moveDir = new Vector2(0,0);
        transform.position = new Vector3(transform.position.x - 3, transform.position.y);
        canAct = false;
    }

    protected void StartControl()
    {
        pData.moveDir = Vector2.left;
        canAct = true;
    }

    public void Unsubscribe(VillageExit villageExit, VillageAndMapManager wMapManager)
    {
        villageExit.OnPlayerLeftVillage  -= StopControl;
        wMapManager.OnPlayerEnterVillage -= StartControl;
    }
}
