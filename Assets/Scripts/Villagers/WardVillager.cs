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

    public bool wardActive = false;
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
    protected override void Awake()
    {
        base.Awake();

        //playerSpecialIsTrigger = true;

        //Spawn Ward but deactivate
        currentWard = Wards[wardName].Spawn().GetComponent<SpawnableSpriteTimeObject>();
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
            currentWard.transform.position = transform.position + wardOffset;
            wardActive = true;
        }
        else
        {
            OnWardUse();
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
    /// What Happens when ward is used
    /// </summary>
    protected abstract void OnWardUse();
    #endregion

    #region Private Methods
    #endregion
}
