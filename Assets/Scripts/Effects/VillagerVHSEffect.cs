using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages it's own VHS effect as well as another, used on villagers 
/// to sync hat up.
/// Created by : Ian Jones      - 30/10/17
/// Updated by : Ian Jones      - 02/04/18
/// </summary>
[ExecuteInEditMode]
public class VillagerVHSEffect : VHSEffect {

    //Sprite Renderer for hat.
    public SpriteRenderer hatRenderer;

    public override void Start()
    {
        base.Start();
    }

    public override void UpdateVHS(bool vhs)
    {
        base.UpdateVHS(vhs);

        if (vhs)
        {
            if (hatRenderer)
            {
                mpb = new MaterialPropertyBlock();
                hatRenderer.GetPropertyBlock(mpb);

                for (int i = 0; i < xScanLines.Length; i++)
                {
                    mpb.SetFloat("_xScanLine" + i, xScanLines[i] * Time.timeScale * TimeObjectManager.pastTimeScale);
                }

                mpb.SetFloat("_yScanLine", yScanLine * Time.timeScale * TimeObjectManager.pastTimeScale);
                mpb.SetFloat("_noiseStrength", noiseStrength);
                mpb.SetFloat("_ScanJitter", scanJitter);
                hatRenderer.SetPropertyBlock(mpb);
            }
        }
        else
        {
            if (hatRenderer)
            {
                mpb = new MaterialPropertyBlock();
                hatRenderer.GetPropertyBlock(mpb);

                for (int i = 0; i < xScanLines.Length; i++)
                {
                    mpb.SetFloat("_xScanLine" + i, 0);
                }

                mpb.SetFloat("_yScanLine", 0);
                mpb.SetFloat("_noiseStrength", 0);
                mpb.SetFloat("_ScanJitter", 0);

                hatRenderer.SetPropertyBlock(mpb);
            }
        }
    }
}
