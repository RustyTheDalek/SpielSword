﻿using UnityEngine;
using System.Collections;

public class Warlock : Villager
{
    #region Public Variables

    public static GameObject wardPrefab;

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
        m_Animator.runtimeAnimatorController = VillagerManager.villagerAnimators[1];


        base.Awake();

        specialType = SpecialType.Press;
        animData.playerSpecialIsTrigger = true;

        wardPrefab = Resources.Load("Ward") as GameObject;
        teleportObj = Instantiate(Resources.Load("Particles/TeleportFX") as GameObject, transform, false);
        teleport = teleportObj.GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    public override void Start ()
    {
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
        if (Game.timeState == TimeState.Forward)
        {
            currentWard = Instantiate(wardPrefab, transform.position, Quaternion.identity) as GameObject;

            wardActive = true;
            animData.canSpecial = false;
        }
    }
}