using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WardVillager : Villager 
{
    #region Public Fields

    /// <summary>
    /// Reference to current ward in use by the Villager
    /// </summary>
    public SpawnableSpriteTimeObject currentWard;

    public bool wardActive;
    #endregion

    #region Protected Fields

    protected string wardName;

    protected Vector3 wardOffset = Vector3.zero;

    #endregion

    #region Private Fields
    #endregion

    static Dictionary<string, GameObject> _Wards;

    static Dictionary<string, GameObject> Wards
    {
        get
        {
            if (_Wards == null)
            {
                _Wards = new Dictionary<string, GameObject>();

                Object[] objs = Resources.LoadAll("Wards");

                GameObject gObj;

                foreach (object obj in objs)
                {
                    if (obj as GameObject != null)
                    {
                        gObj = (GameObject)obj;

                        _Wards.Add(gObj.name, gObj);
                        _Wards[gObj.name].CreatePool(50);
                    }
                }
            }

            return _Wards;
        }
    }

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();

        animData["PlayerSpecialIsTrigger"] = true;

        //Spawn Ward but deactivate
        currentWard = Wards[wardName].Spawn().GetComponent<SpawnableSpriteTimeObject>();
        currentWard.creator = this;
        currentWard.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    public override void OnSpecial(bool _PlayerSpecial)
    {
        if (!wardActive)
        {
            animData["PlayerSpecial"] = _PlayerSpecial;
        }
        else
        {
            OnWardActive(_PlayerSpecial);
        } 
    }

    /// <summary>
    /// Called by the Animator to spawn the ward at the correct time
    /// </summary>
    public void SpawnWard()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            currentWard.gameObject.SetActive(true);
            currentWard.transform.position = transform.position + wardOffset;
            wardActive = true;
            animData["CanSpecial"] = false;
        }
    }

    public override void FireProjectile()
    {
        if (villagerState == VillagerState.PresentVillager)
        {
            Debug.Log("Warlock Ranged Attack");

            rangedAtk = Projectile.Spawn(rangedTrans.position);

            float direction = rangedTrans.position.x - transform.position.x;

            rangedAtk.GetComponent<VillagerAttack>().lifeTime = 2;
            rangedAtk.GetComponent<VillagerAttack>().damage = 1;
            rangedAtk.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction)
                , 0) * rangedProjectileStrength, ForceMode2D.Impulse);
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// What happens when Special is activated and Ward is placed
    /// </summary>
    /// <param name="_PlayerSpecial"></param>
    protected abstract void OnWardActive(bool _PlayerSpecial);

    #endregion

    #region Private Methods
    #endregion
}
