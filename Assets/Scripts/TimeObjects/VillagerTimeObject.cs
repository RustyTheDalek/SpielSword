using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTimeObject : BaseTimeObject<VillagerFrameData>
{
    Villager villager;

    protected PlatformerCharacter2D m_Character;

    VillagerAnimData vAnimData;

    protected override void Start()
    {
        base.Start();

        villager = GetComponent<Villager>();
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    protected override void PlayFrame()
    {
        vAnimData = new VillagerAnimData();

        vAnimData.move = frames[currentFrame].move;
        //villager.animData.jump = actions[Game.t].jump;

        switch (villager.villagerAttackType)
        {
            case AttackType.Melee:
                vAnimData.meleeAttack = frames[currentFrame].meleeAttack;
                villager.CanAttack(vAnimData.meleeAttack);
                break;

            case AttackType.Ranged:
                vAnimData.rangedAttack = frames[currentFrame].rangedAttack;
                villager.CanAttack(vAnimData.rangedAttack);
                break;
        }

        vAnimData.playerSpecial = frames[currentFrame].special;
        vAnimData.canSpecial = frames[currentFrame].canSpecial;
        vAnimData.dead = frames[currentFrame].dead;

        m_Character.Move(vAnimData);

        transform.position = frames[currentFrame].m_Position;
        transform.rotation = frames[currentFrame].m_Rotation;

        gameObject.SetActive(frames[currentFrame].enabled);

        currentFrame++;
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
                tempFrame.meleeAttack = villager.animData.meleeAttack;
                break;

            case AttackType.Ranged:
                tempFrame.rangedAttack = villager.animData.rangedAttack;
                break;
        }

        tempFrame.health = villager.health;
        tempFrame.special = villager.animData.playerSpecial;
        tempFrame.canSpecial = villager.animData.canSpecial;
        tempFrame.dead = !villager.alive;

        frames.Add(tempFrame);
    }

    protected new void Playback()
    {
        if (totalFrames > 0)
        {
            //If Game time matches start frame begin playback
            if (Game.t == frames[0].timeStamp && !replaying)
            {
                replaying = true;
            }

            if (replaying)
            {
                PlayFrame();
            }

            if (Game.t == frames[frames.Count-1].timeStamp && replaying)
            {
                replaying = false;
            }
        }
    }

    protected override void OnPast()
    {
        base.OnPast();
        villager.villagerState = VillagerState.PastVillager;
    }
}
