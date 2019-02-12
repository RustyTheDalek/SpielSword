using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : WardVillager
{
    #region Public Fields

    public string summonName;

    public Summon currentSummon;

    public bool summonActive = false;

    #endregion

    #region Protected Fields

    protected Vector3 summonOffset;

    #endregion

    #region Private Fields
    #endregion

    #region Unity Methods
    protected override void Awake()
    { 
        wardOffset = new Vector3(-2, 2, 0);
        summonOffset = new Vector3(2, 0, 0);

        base.Awake();

        //Spawn Ward but deactivate
        currentSummon = abilities.LoadAsset<Summon>(summonName).Spawn();
        currentSummon.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    public void UseSummon()
    {
        if(!summonActive)
        {
            currentSummon.gameObject.SetActive(true);
            currentSummon.transform.position = Sprite.transform.position + summonOffset;

            //We want the Summon to go in the direction the player is moving or looking
            if (moveDir != Vector2.zero)
                currentSummon.moveDir = moveDir;
            else
                currentSummon.moveDir = m_Ground.FacingRight ? Vector2.right : Vector2.left;

            summonActive = true;
        }
        else
        {
            currentSummon.Kill();
            currentSummon = abilities.LoadAsset<Summon>(summonName).Spawn();
        }
    }

    #endregion

    #region Protected Methods

    protected override void OnWardUse()
    {
        currentWard.transform.position = transform.position + wardOffset;
        currentWard.spawnableTimer = currentWard.spawnableLife;
    }

    #endregion

    #region Private Methods
    #endregion
}
