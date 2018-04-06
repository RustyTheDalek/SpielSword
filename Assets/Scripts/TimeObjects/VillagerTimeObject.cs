using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTimeObject : SpriteTimeObject
{
    #region Public Variables

    public bool attackStart,
                endFinish,
                endRecorded,
                deathOrMarty = true; //Whether te villager is going to die or fade from existence

    #endregion

    #region Protected Variables

    protected VillagerCharacter2D m_Villager;

    protected SpriteRenderer m_HatSprite;

    #endregion

    #region Private Variables

    Villager villager;
    Hashtable animData;

    private VillagerFrameData tempFrame;
    private List<VillagerFrameData> vFrames = new List<VillagerFrameData>();

    #endregion

    protected override void Awake()
    {
        base.Awake();

        villager = GetComponent<Villager>();
        m_Villager = GetComponent<VillagerCharacter2D>();
        m_HatSprite = GetComponentsInChildren<VHSEffect>()[1].GetComponent<SpriteRenderer>();

        if (GetComponent<VHSEffect>())
        {
            vhsEffect = GetComponent<VHSEffect>();
        }
        else
        {
            vhsEffect = gameObject.AddComponent<VHSEffect>();
        }

        animData = new Hashtable
        {
            { "Move", 0 },
            { "Dead", false },
            { "MeleeAttack", false },
            { "RangedAttack", false },
            { "Jump", false },
            { "PlayerSpecial", false },
            { "CanSpecial", true},
            { "PlayerSpecialIsTrigger", false},
            { "Martyed", false},
        };
    }

    private void Start()
    {
        OnPlayFrame += OnVillagerPlayFrame;
        OnTrackFrame += OnVillagerTrackFrame;
        OnFinishPlayback += OnVillagerFinishPlayback;
        OnStartReverse += OnVillagerStartReverse;
        OnFinishReverse += OnVillagerFinishReverse;
    }

    protected void OnVillagerPlayFrame()
    {
        if (Tools.WithinRange(currentFrame, vFrames))
        {

            m_HatSprite.color = new Color(m_HatSprite.color.r,
                                            m_HatSprite.color.g,
                                            m_HatSprite.color.b,
                                            .75f);

            //villager.health = vFrames[currentFrame].health;
            animData["Move"] = vFrames[currentFrame].move;

            switch (Game.timeState)
            {
                case TimeState.Forward:

                    //villager.animData.jump = actions[Game.t].jump;

                    switch (villager.attackType)
                    {
                        case AttackType.Melee:
                            //vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                            m_Villager.CanAttack((bool)animData["MeleeAttack"]);
                            animData["MeleeAttack"] = vFrames[currentFrame].meleeAttack;
                            break;

                        case AttackType.Ranged:
                            //vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                            m_Villager.CanAttack((bool)animData["RangedAttack"]);
                            animData["RangedAttack"] = vFrames[currentFrame].rangedAttack;
                            break;
                    }

                    animData["PlayerSpecial"] = vFrames[currentFrame].special;
                    animData["CanSpecial"] = vFrames[currentFrame].canSpecial;
                    animData["Dead"] = vFrames[currentFrame].dead;
                    animData["Martyed"] = vFrames[currentFrame].marty;
                    //animData["LocalScale"] = vFrames[currentFrame].scale;

                    m_Villager.Move(animData);

                    break;

                case TimeState.Backward:

                    m_Sprite.sprite = AssetManager.VillagerSprites[vFrames[currentFrame].spriteName];
                    villager.hat.localPosition = vFrames[currentFrame].hatPos;
                    transform.localScale = vFrames[currentFrame].scale;

                    break;
            }
        }
    }

    protected void OnVillagerTrackFrame()
    {
        tempFrame = new VillagerFrameData()
        {
            move = villager.xDir,
            //health = villager.health,

            spriteName = m_Sprite.sprite.name,
            hatPos = villager.hat.localPosition,
            scale = transform.localScale
        };

        switch (villager.attackType)
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
        tempFrame.special = (bool)villager.animData["PlayerSpecial"];
        tempFrame.canSpecial = (bool)villager.animData["CanSpecial"];
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
        //villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials["VHSSprite"];
    }

    protected void OnVillagerFinishPlayback()
    {
        Debug.Log("Villager Finished playback, dying");
        animData["Dead"] = deathOrMarty;
        animData["Martyed"] = !deathOrMarty;
        animData["Move"] = 0;
        animData["CanSpecial"] = false;

        m_Villager.Move(animData);
    }

    protected void OnVillagerFinishReverse()
    {
        GetComponent<Rigidbody2D>().simulated = true;


        m_HatSprite.material = AssetManager.SpriteMaterials["Sprite"];


        m_Sprite.color = new Color(m_Sprite.color.r,
                                        m_Sprite.color.g,
                                        m_Sprite.color.b,
                                        .5f);

        villager.hat.GetComponent<SpriteRenderer>().color = new Color(  villager.hat.GetComponent<SpriteRenderer>().color.r,
                                                                        villager.hat.GetComponent<SpriteRenderer>().color.g,
                                                                        villager.hat.GetComponent<SpriteRenderer>().color.b,
                                                                        .5f);
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
            sFrames.RemoveAt(i);
            vFrames.RemoveAt(i);
        }
    }

    protected void OnVillagerStartReverse()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        m_HatSprite.material = AssetManager.SpriteMaterials["VHSSprite"];
        villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials["VHSSprite"];
    }

}
