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
    public GameObject currentWard;

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
        currentWard = abilities.LoadAsset<GameObject>(wardName + "Ward").Spawn();
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
            currentWard.GetComponent<TimeObject>().tObjectState = TimeObjectState.Present;
            wardActive = true;
        }
        else
        {
            OnWardUse();
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
