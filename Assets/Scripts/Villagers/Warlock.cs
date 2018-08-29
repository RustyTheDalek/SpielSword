using UnityEngine;
using System.Collections;

public class Warlock : WardVillager
{
    #region Public Variables

    //public GameObject currentWard;

    //public bool wardActive;

    #endregion

    #region Protected Variables

    protected ParticleSystem teleportFX;

    #endregion

    #region Private Variables

    #endregion

    protected override void Awake()
    {
        specialType = SpecialType.Press;

        base.Awake();

        teleportFX = currentWard.GetComponentInChildren<ParticleSystem>();
    }

    protected override void OnWardUse()
    {
        Debug.Log("Teleporting");
        transform.position = currentWard.transform.position;
        teleportFX.Play();
    }

    //TODO:Ref Ward Villager
    public override void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Warlock Ranged Attack");

            rangedAtk = abilities.LoadAsset<GameObject>("Range").Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<VillagerAttack>().lifeTime = .25f;
            rangedAtk.GetComponent<VillagerAttack>().damage = damageMult;
            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }
}
