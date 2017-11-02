using UnityEngine;
using System.Collections;

public class Warlock : Villager
{
    #region Public Variables

    public GameObject currentWard;

    public bool wardActive;

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
        animData.playerSpecialIsTrigger = true;

        teleportObj = Instantiate(Resources.Load("Particles/TeleportFX") as GameObject, transform, false);
        teleport = teleportObj.GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    public override void Start ()
    {
        m_Animator.runtimeAnimatorController = AssetManager.VillagerAnimators[1];

        base.Start();

        villagerAttackType = AttackType.Ranged;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void OnSpecial(bool _PlayerSpecial)
    {
        if (!wardActive)
        {
            animData.playerSpecial = _PlayerSpecial;
        }
        else
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
    }

    /// <summary>
    /// Called by the Animator to spawn the ward at the correct time
    /// </summary>
    public void SpawnWard()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            currentWard = AssetManager.Ward.Spawn(transform.position);

            wardActive = true;
            animData.canSpecial = false;
        }
    }

    public override void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Warlock Ranged Attack");

            rangedAtk = AssetManager.Projectile.Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<VillagerAttack>().lifeTime = .25f;
            rangedAtk.GetComponent<VillagerAttack>().damage = 2;
            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);

            VillagerManager.attacks.Add(rangedAtk.GetComponent<SpawnableSpriteTimeObject>());
        }
    }
}
