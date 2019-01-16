using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WardVillager : Villager 
{
    #region Public Fields

    public string wardName;

    /// <summary>
    /// Reference to current ward in use by the Villager
    /// </summary>
    public SpawnableSpriteTimeObject currentWard;

    public bool wardActive = false;
    #endregion

    #region Protected Fields

    protected Vector3 wardOffset = Vector3.zero;

    #endregion

    #region Private Fields
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();

        //playerSpecialIsTrigger = true;

        //Spawn Ward but deactivate
        GameObject temp  = abilities.LoadAsset<GameObject>(wardName + "Ward").Spawn();
        currentWard = temp.GetComponent<SpawnableSpriteTimeObject>();
        currentWard.creator = this;
        currentWard.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called by Animator to Spawn or use Ward at correct time
    /// </summary>
    public void UseWard()
    {
        if (!wardActive)
        {
            currentWard.gameObject.SetActive(true);
            currentWard.transform.position = Sprite.transform.position + wardOffset;
            wardActive = true;
        }
        else
        {
            OnWardUse();
        }
    }

    //TODO:Remove this naughtyness should have one fire function and the variable be 
    //on the Villager or maybe spawn different range projeticle prefabs
    public override void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Warlock Ranged Attack");

            rangedAtk = abilities.LoadAsset<GameObject>("Range").Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - m_rigidbody.transform.position.x;

            rangedAtk.GetComponent<VillagerAttack>().lifeTime = 2;
            rangedAtk.GetComponent<VillagerAttack>().damage = 1;
            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// What Happens when ward is used
    /// </summary>
    protected abstract void OnWardUse();
    #endregion

    #region Private Methods
    #endregion
}
