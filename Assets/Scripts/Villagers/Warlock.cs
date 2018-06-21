using UnityEngine;
using System.Collections;

public class Warlock : WardVillager
{
    #region Public Variables

    //public GameObject currentWard;

    //public bool wardActive;

    #endregion

    #region Protected Variables

    protected GameObject teleportObj;
    protected ParticleSystem teleport;

    #endregion

    #region Private Variables

    #endregion

    public override void Awake()
    {
        base.Awake();

        specialType = SpecialType.Press;

        teleportObj = Instantiate(Resources.Load("Particles/TeleportFX") as GameObject, transform, false);
        teleport = teleportObj.GetComponent<ParticleSystem>();

        wardName = "WarlockWard";
    }


    protected override void OnWardActive(bool _PlayerSpecial)
    {
        //TODO: Finalise functionality
        //If the Player presses the button once the Ward is active do the teleport
        //In future maybe destroyd current copy?
        if (_PlayerSpecial)
        {
            Debug.Log("Teleporting");
            transform.position = currentWard.transform.position;
            teleport.Play();
        }
    }

    public override void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Warlock Ranged Attack");

            rangedAtk = Projectile.Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<VillagerAttack>().lifeTime = .25f;
            rangedAtk.GetComponent<VillagerAttack>().damage = 2;
            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }
}
