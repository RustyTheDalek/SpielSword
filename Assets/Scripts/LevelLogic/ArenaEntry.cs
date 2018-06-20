using UnityEngine;

/// <summary>
/// Tracks when Player enters Arena and fight can begin
/// Created by Ian Jones : 03/04/18
/// </summary>
public class ArenaEntry : MonoBehaviour {

    public delegate void PlayerEnteredArenaEvent();
    public event PlayerEnteredArenaEvent OnPlayerEnterArena;

    bool playerEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Villager" &&
            !playerEntered)
        {
            Debug.Log("Player entered Arena");

            if(OnPlayerEnterArena != null)
                OnPlayerEnterArena();

            //OneShot event
            playerEntered = true;
        }
        else
        {
            Debug.Log("Something entered me but wasn't a villager");
        }
    }
}
