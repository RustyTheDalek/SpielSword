using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : WardVillager
{
    #region Public Fields
    #endregion

    #region Protected Fields
    #endregion

    #region Private Fields
    #endregion

    #region Unity Methods
    protected override void Awake()
    { 
        wardOffset = new Vector3(-2, 2, 0);

        base.Awake();
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
