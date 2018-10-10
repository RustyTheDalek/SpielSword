using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls logic for Villager playback, specifically using the animator and sprites
/// Created by : Ian Jones - 19/03/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public class VillagerTimeObject : AnimatorTimeObject
{
    #region Public Variables

    #endregion

    #region Protected Variables

    protected GroundCharacter2D m_Ground;

    protected SpriteRenderer m_HatSprite;

    #endregion

    #region Private Variables

    Villager villager;

    private VillagerFrameData tempVFrame;
    private List<VillagerFrameData> vFrames = new List<VillagerFrameData>();

    #endregion

    protected override void Awake()
    {
        base.Awake();

        villager = GetComponent<Villager>();
        m_Ground = GetComponent<GroundCharacter2D>();
        m_HatSprite = transform.Find("Hat").GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        tObjectState = TimeObjectState.Present;
        startFrame = (int)TimeObjectManager.t;
    }

    protected override void Start()
    {
        base.Start();

        OnPlayFrame += OnVillagerPlayFrame;
        OnTrackFrame += OnVillagerTrackFrame;
        OnStartPlayback += OnVillagerStartPlayback;
        OnFinishPlayback += OnVillagerFinishPlayback;
        OnStartReverse += OnVillagerStartReverse;
    }

    protected void OnVillagerPlayFrame()
    {
        if (vFrames.WithinRange(currentFrame))
        {
            villager.hat.transform.localPosition = vFrames[(int)currentFrame].hatPos;
            transform.localScale = vFrames[(int)currentFrame].scale;
            villager.portal.transform.localPosition = vFrames[(int)currentFrame].portalPos;
            villager.portal.transform.localScale = vFrames[(int)currentFrame].portalScale;

            if(villager.melee)
                villager.melee.enabled = vFrames[(int)currentFrame].meleeColliderEnabled;
        }
    }

    protected void OnVillagerTrackFrame()
    {
        tempVFrame = new VillagerFrameData()
        {
            hatPos = villager.hat.transform.localPosition,
            scale = transform.localScale,
            portalPos = villager.portal.transform.localPosition,
            portalScale = villager.portal.transform.localScale,
            meleeColliderEnabled = villager.melee ? villager.melee.enabled : false
        };

        vFrames.Add(tempVFrame);
    }
    /// <summary>
    /// What happens when a Villager become a past incarnation
    /// </summary>
    protected override void OnPast()
    {
        //base.OnPast();
        villager.villagerState = VillagerState.PastVillager;
        //villager.hat.GetComponentInChildren<SpriteRenderer>().material = AssetManager.SpriteMaterials["VHSSprite"];
    }

    protected void OnVillagerStartPlayback()
    {
        m_Character.RestoreHealth();
    }

    protected void OnVillagerStartReverse()
    {
        m_HatSprite.enabled = true;
        m_Sprite.enabled = true;
    }

    protected void OnVillagerFinishPlayback()
    {
        m_Sprite.enabled = false;
        m_HatSprite.enabled = false;
    }

    //public void SetMartyPoint()
    //{
    //    deathOrMarty = false;
    //    tempFrame = vFrames[currentFrame];
    //    tempFrame.marty = true;
    //    vFrames[currentFrame] = tempFrame;

    //    finishFrame = bFrames[currentFrame].timeStamp;

    //    for (int i = currentFrame+1; i < bFrames.Count; i++)
    //    {
    //        bFrames.RemoveAt(i);
    //        sFrames.RemoveAt(i);
    //        vFrames.RemoveAt(i);
    //    }
    //}

    private void OnDestroy()
    {
        OnStartPlayback -= OnVillagerStartPlayback;

        OnPlayFrame -= OnVillagerPlayFrame;
        OnTrackFrame -= OnVillagerTrackFrame;
        OnFinishPlayback -= OnVillagerFinishPlayback;
        OnStartReverse -= OnVillagerStartReverse;
    }
}
