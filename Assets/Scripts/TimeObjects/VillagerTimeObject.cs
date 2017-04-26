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
                deathFinish,
                deathRecorded;

    SpriteRenderer _SRenderer;

    protected override void Start()
    {
        base.Start();

        villager = GetComponent<Villager>();
        m_Character = GetComponent<PlatformerCharacter2D>();
        _SRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void PlayFrame()
    {
        vAnimData = new VillagerAnimData();

        villager.health = frames[currentFrame].health;  
        vAnimData.move = frames[currentFrame].move;

        switch (Game.timeState)
        {
            case TimeState.Forward:

                //villager.animData.jump = actions[Game.t].jump;

                switch (villager.villagerAttackType)
                {
                    case AttackType.Melee:
                        //vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                        villager.CanAttack(vAnimData.meleeAttack);
                        vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                        break;

                    case AttackType.Ranged:
                        //vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                        villager.CanAttack(vAnimData.rangedAttack);
                        vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                        break;
                }

                vAnimData.playerSpecial = frames[currentFrame].special;
                vAnimData.canSpecial = frames[currentFrame].canSpecial;
                vAnimData.dead = frames[currentFrame].dead;
                transform.localScale = frames[currentFrame].scale;

                m_Character.Move(vAnimData);

                break;

            case TimeState.Backward:

                _SRenderer.sprite = AssetManager.villagerSprites[frames[currentFrame].spriteName];
                villager.hat.localPosition = frames[currentFrame].hatPos;
                transform.localScale = frames[currentFrame].scale;

                break;
        }

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
        tempFrame.health = villager.health;

        tempFrame.spriteName = _SRenderer.sprite.name;
        tempFrame.hatPos = villager.hat.localPosition;
        tempFrame.scale = transform.localScale;

        switch (villager.villagerAttackType)
        {
            case AttackType.Melee:
                //tempFrame.meleeAttack = villager.animData.meleeAttack;
                tempFrame.meleeAttack = attackStart;
                break;

            case AttackType.Ranged:
                //tempFrame.rangedAttack = villager.animData.rangedAttack;
                tempFrame.rangedAttack = attackStart;
                break;
        }
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
        deathFinish = false;
    }

    protected override void OnStartPlayback()
    {
        //Debug.Break();
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
        //transform.localScale = Vector3.one;
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
    }

    protected override void OnFinishPlayback()
    {
        vAnimData = new VillagerAnimData();
        vAnimData.dead = true;
        vAnimData.move = 0;
        vAnimData.jump = false;
        m_Character.Move(vAnimData);
    }
}
