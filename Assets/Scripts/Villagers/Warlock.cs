using UnityEngine;
using System.Collections;

public class Warlock : WardVillager
{
    #region Public Variables
    #endregion

    #region Protected Variables

    protected ParticleSystem teleportFX;

    #endregion

    #region Private Variables

    #endregion

    protected override void Awake()
    {
        rangeName = "WarlockRange";

        base.Awake();

        //teleportFX = currentWard.GetComponentInChildren<ParticleSystem>();
    }

    protected override void OnWardUse()
    {
        Debug.Log("Teleporting");
        //m_rigidbody.position = currentWard.transform.position;
        teleportFX.Play();
    }
}
