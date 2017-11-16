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
    public override void Awake()
    {
        base.Awake();

        wardOffset = new Vector3(-2, 2, 0);

        wardName = "ShamanTotem";
    }

    #endregion

    #region Protected Methods

    protected override void OnWardActive(bool _PlayerSpecial)
    {
        if (_PlayerSpecial)
        {
            currentWard.transform.position = transform.position + wardOffset;
        }
    }

    #endregion

    #region Private Methods
    #endregion
}
