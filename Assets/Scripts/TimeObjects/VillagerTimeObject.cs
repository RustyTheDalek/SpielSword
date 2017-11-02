using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTimeObject : SpriteTimeObject
{
    Villager villager;

    protected VillagerCharacter2D m_Character;

    VillagerAnimData vAnimData;

    public bool attackStart,
                endFinish,
                endRecorded,
                deathOrMarty = true; //Whether te villager is going to die or fade from existence

    SpriteRenderer _SRenderer;

    private VillagerFrameData tempFrame;
    private List<VillagerFrameData> vFrames = new List<VillagerFrameData>();

    protected override void Start()
    {
        base.Start();

        villager = GetComponent<Villager>();
        m_Character = GetComponent<VillagerCharacter2D>();
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
        base.PlayFrame();

        if (Tools.WithinRange(currentFrame, vFrames))
        {
            vAnimData = new VillagerAnimData();

            villager.health = vFrames[currentFrame].health;
            vAnimData.move = vFrames[currentFrame].move;

            switch (Game.timeState)
            {
                case TimeState.Forward:

                    //villager.animData.jump = actions[Game.t].jump;

                    switch (villager.villagerAttackType)
                    {
                        case AttackType.Melee:
                            //vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                            villager.CanAttack(vAnimData.meleeAttack);
                            vAnimData.meleeAttack = vFrames[currentFrame].meleeAttack;
                            break;

                        case AttackType.Ranged:
                            //vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                            villager.CanAttack(vAnimData.rangedAttack);
                            vAnimData.rangedAttack = vFrames[currentFrame].rangedAttack;
                            break;
                    }

                    vAnimData.playerSpecial = vFrames[currentFrame].special;
                    vAnimData.canSpecial = vFrames[currentFrame].canSpecial;
                    vAnimData.dead = vFrames[currentFrame].dead;
                    vAnimData.martyed = vFrames[currentFrame].marty;
                    transform.localScale = vFrames[currentFrame].scale;

                    m_Character.Move(vAnimData);

                    break;

                case TimeState.Backward:

                    _SRenderer.sprite = AssetManager.VillagerSprites[vFrames[currentFrame].spriteName];
                    villager.hat.localPosition = vFrames[currentFrame].hatPos;
                    transform.localScale = vFrames[currentFrame].scale;

                    break;
            }
        }
    }

    protected override void TrackFrame()
    {
        base.TrackFrame();

        tempFrame = new VillagerFrameData()
        {
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

        if (!endRecorded && endFinish)
        {
            if (deathOrMarty)
            {
                tempFrame.deathEnd = endFinish;
            }
            else
            {
                tempFrame.marty = endFinish;
            }
            endRecorded = true;
        }

        vFrames.Add(tempFrame);

        attackStart = false;
        endFinish = false;
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
            dead = deathOrMarty,
            martyed = !deathOrMarty,
            move = 0,
        };

        m_Character.Move(vAnimData);
    }

    protected override void OnFinishReverse()
    {
        base.OnFinishReverse();

        _SRenderer.material = AssetManager.SpriteMaterials[0];
        villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials[0];
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
        vhsEffect.enabled = true;
    }

    public void SetMartyPoint()
    {
        deathOrMarty = false;
        tempFrame = vFrames[currentFrame];
        tempFrame.marty = true;
        vFrames[currentFrame] = tempFrame;

        finishFrame = bFrames[currentFrame].timeStamp;

        for (int i = currentFrame+1; i < bFrames.Count; i++)
        {
            bFrames.RemoveAt(i);
            vFrames.RemoveAt(i);
            sFrames.RemoveAt(i);
        }
    }
}
