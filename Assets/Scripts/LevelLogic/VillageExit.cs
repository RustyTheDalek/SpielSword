using UnityEngine;

/// <summary>
/// Tracks when Player Leaves Village
/// Created by Ian Jones : 09/04/18
/// </summary>
public class VillageExit : MonoBehaviour {

    public delegate void PlayerLeftVillageEvent();
    public static event PlayerLeftVillageEvent OnPlayerLeftVillage;

    bool playerEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Villager" &&
            !playerEntered)
        {
            Debug.Log("Player Left Village");

            if(OnPlayerLeftVillage != null)
                OnPlayerLeftVillage();

            //OneShot event
            playerEntered = true;
        }
        else
        {
            Debug.Log("Something entered me but wasn't a villager");
        }
    }
}
