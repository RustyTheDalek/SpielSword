using UnityEngine;

/// <summary>
/// Script for registering Boss attacks
/// Created     : GG16 
/// Updated by  : Ian Jones - 02/04/18
/// </summary>
public class BossAttack : MonoBehaviour
{
    public bool attacking = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //BossAttack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && this.enabled
            && !LevelManager.GodMode)
        {
            coll.gameObject.GetComponent<Villager>().OnHit();
        }
    }
}
