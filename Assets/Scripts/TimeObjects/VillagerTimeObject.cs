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

    VHSEffect vhsEffect;

    protected override void Start()
    {
        base.Start();

        villager = GetComponent<Villager>();
        m_Character = GetComponent<PlatformerCharacter2D>();
        _SRenderer = GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }

        //tObjectState = TimeObjectState.Void;
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

                _SRenderer.sprite = AssetManager.VillagerSprites[frames[currentFrame].spriteName];
                villager.hat.localPosition = frames[currentFrame].hatPos;
                transform.localScale = frames[currentFrame].scale;

                break;
        }

        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame += Game.GameScale;
    }

    protected override void TrackFrame()
    {
        tempFrame = new VillagerFrameData()
        {
            m_Position = transform.position,
            m_Rotation = transform.rotation,

            timeStamp = Game.t,

            enabled = gameObject.activeSelf,

            move = villager.xDir,
            health = villager.health,

            spriteName = _SRenderer.sprite.name,
            hatPos = villager.hat.localPosition,
            scale = transform.localScale
        };

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
        tempFrame.dead = !villager.Alive;

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

    protected override void OnFinishPlayback()
    {
        vAnimData = new VillagerAnimData()
        {
            dead = true,
            move = 0,
        };

        m_Character.Move(vAnimData);
    }

    protected override void OnFinishReverse()
    {
        base.OnFinishReverse();

        _SRenderer.material = AssetManager.SpriteMaterials[0];
        villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials[0];
        villager.hat.GetComponent<VHSEffect>().enabled = false;
        vhsEffect.enabled = false;

        _SRenderer.color = new Color(   _SRenderer.color.r,
                                        _SRenderer.color.g,
                                        _SRenderer.color.b,
                                        .5f);

        villager.hat.GetComponent<SpriteRenderer>().color = new Color(  villager.hat.GetComponent<SpriteRenderer>().color.r,
                                                                        villager.hat.GetComponent<SpriteRenderer>().color.g,
                                                                        villager.hat.GetComponent<SpriteRenderer>().color.b,
                                                                        .5f);
    }

    protected override void OnStartReverse()
    {
        base.OnStartReverse();

        _SRenderer.material = AssetManager.SpriteMaterials[1];
        villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials[1];
        villager.hat.GetComponent<VHSEffect>().enabled = true;
        vhsEffect.enabled = true;
    }
}
