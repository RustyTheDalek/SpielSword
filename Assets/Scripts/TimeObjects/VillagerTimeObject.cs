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

    protected override void Start()
    {
        base.Start();

        OnPlayFrame += OnVillagerPlayFrame;
        OnTrackFrame += OnVillagerTrackFrame;
        OnFinishReverse += OnVillagerFinishReverse;
    }

    protected void OnVillagerPlayFrame()
    {
        if (Tools.WithinRange(currentFrame, vFrames))
        {
            villager.hat.transform.localPosition = vFrames[currentFrame].hatPos;
            transform.localScale = vFrames[currentFrame].scale;
        }
    }

    protected void OnVillagerTrackFrame()
    {
        tempVFrame = new VillagerFrameData()
        {
            hatPos = villager.hat.transform.localPosition,
            scale = transform.localScale
        };

        vFrames.Add(tempVFrame);
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

    protected void OnVillagerFinishReverse()
    {


        m_Sprite.color = new Color(m_Sprite.color.r,
                                        m_Sprite.color.g,
                                        m_Sprite.color.b,
                                        .5f);

        m_HatSprite.color = new Color(  villager.hat.GetComponent<SpriteRenderer>().color.r,
                                                                        villager.hat.color.g,
                                                                        villager.hat.color.b,
                                                                        .5f);
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
        OnFinishReverse -= OnVillagerFinishReverse;

        OnPlayFrame -= OnVillagerPlayFrame;
        OnTrackFrame -= OnVillagerTrackFrame;
    }
}
