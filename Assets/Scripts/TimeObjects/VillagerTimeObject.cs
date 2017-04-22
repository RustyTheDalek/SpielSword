using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTimeObject : BaseTimeObject<VillagerFrameData>
{
    Villager villager;

    protected PlatformerCharacter2D m_Character;

    VillagerAnimData vAnimData;

    public bool attackStart, 
                attackFinish,
                deathFinish,
                deathRecorded;

    protected override void Start()
    {
        base.Start();

        villager = GetComponent<Villager>();
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    protected override void PlayFrame()
    {
        vAnimData = new VillagerAnimData();

        villager.health = frames[currentFrame].health;  
        vAnimData.move = frames[currentFrame].move;
        //villager.animData.jump = actions[Game.t].jump;

        switch (villager.villagerAttackType)
        {
            case AttackType.Melee:
                //vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                villager.CanAttack(vAnimData.meleeAttack);
                vAnimData.meleeAttackEnd = frames[currentFrame].meleeAttackEnd;
                vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                break;

            case AttackType.Ranged:
                //vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                villager.CanAttack(vAnimData.rangedAttack);
                vAnimData.rangedAttackEnd = frames[currentFrame].rangedAttackEnd;
                vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                break;
        }

        vAnimData.playerSpecial = frames[currentFrame].special;
        vAnimData.canSpecial = frames[currentFrame].canSpecial;
        vAnimData.dead = frames[currentFrame].dead;
        vAnimData.deathEnd = frames[currentFrame].deathEnd;

        m_Character.Move(vAnimData);

        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame += (int)Game.timeState;
    }

    protected override void TrackFrame()
    {

        tempFrame = new VillagerFrameData();

        tempFrame.m_Position = transform.position;
        tempFrame.m_Rotation = transform.rotation;

        tempFrame.timeStamp = Game.t;

        tempFrame.enabled = gameObject.activeSelf;

        tempFrame.move = villager.xDir;

        switch (villager.villagerAttackType)
        {
            case AttackType.Melee:
                //tempFrame.meleeAttack = villager.animData.meleeAttack;
                tempFrame.meleeAttack = attackStart;
                tempFrame.meleeAttackEnd = attackFinish;

                break;

            case AttackType.Ranged:
                //tempFrame.rangedAttack = villager.animData.rangedAttack;
                tempFrame.rangedAttack = attackStart;
                tempFrame.rangedAttackEnd = attackFinish;

                break;
        }

        tempFrame.health = villager.health;
        tempFrame.special = villager.animData.playerSpecial;
        tempFrame.canSpecial = villager.animData.canSpecial;
        tempFrame.dead = !villager.alive;

        if (!deathRecorded && deathFinish)
        {
            tempFrame.deathEnd = deathFinish;
            deathRecorded = true;
        }

        frames.Add(tempFrame);

        attackStart = false;
        attackFinish = false;
        deathFinish = false;

    }

    protected override void OnStartPlayback()
    {
        villager.ForwardVillager();
    }

    /// <summary>
    /// What happens when a Villager become a past incarnation
    /// </summary>
    protected override void OnPast()
    {
        base.OnPast();
        villager.villagerState = VillagerState.PastVillager;
        OnStartReverse();
    }

    protected override void OnStartReverse()
    {
        villager.ReverseVillager();
    }

    protected override void OnFinishPlayback()
    {
        vAnimData = new VillagerAnimData();
        vAnimData.dead = true;
        m_Character.Move(vAnimData);
    }
}
