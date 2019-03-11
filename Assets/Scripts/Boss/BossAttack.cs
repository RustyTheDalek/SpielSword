using UnityEngine;

/// <summary>
/// Script for registering Boss attacks
/// Created     : GG16 
/// Updated by  : Ian Jones - 02/04/18
/// </summary>
public class BossAttack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        //BossAttack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && enabled
            && !LevelManager.GodMode)
        {
            Debug.Log("Boss Hit : " + coll.gameObject.name);
            coll.gameObject.GetComponentInParent<LivingObject>().OnHit(
                coll.transform.position.PointTo(transform.position));
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //BossAttack can only damage Villagers, has to be enabled and God mode off for 
        //obvious reasons
        if (coll.gameObject.layer == (LayerMask.NameToLayer("Villager")) && enabled
            && !LevelManager.GodMode)
        {
            Debug.Log("Boss Hit : " + name);
            coll.gameObject.GetComponentInParent<LivingObject>().OnHit(
                coll.transform.position.PointTo(transform.position));
        }
    }
}
