using UnityEngine;
using System.Collections;

public class Warlock : Villager
{
    public static GameObject wardPrefab;

    public GameObject currentWard;

    public bool wardActive;

    float holdTimer = 0;

    public override void Awake()
    {
        base.Awake();

        wardPrefab = Resources.Load("Ward") as GameObject;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	//public override void Update ()
 //   {
 //       base.Update();

 //       switch (villagerState)
 //       {
 //           case VillagerState.CurrentVillager:

 //               //If the Ward is active and the player presses uses the special button 
 //               //again they need to teleport
 //               if (animData.playerSpecial && wardActive)
 //               {
 //                   transform.position = currentWard.transform.position;
 //               }
 //               break;
 //       }
	//}

    public override void OnSpecial(bool playerSpecial)
    {
        if (!wardActive)
        {
            animData.playerSpecial = playerSpecial;
        }
        else
        {
            //NOT FINISHED
            //If the Player presses the button once the Ward is active do the teleport
            //In future maybe destroyd current copy?
            if (playerSpecial)
            {
                Debug.Log("Teleporting");
                transform.position = currentWard.transform.position;
            }
        }
    }

    public void SpawnWard()
    {
        currentWard = Instantiate(wardPrefab, transform.position, Quaternion.identity) as GameObject;

        wardActive = true;
        animData.canSpecial = false;
    }
}
