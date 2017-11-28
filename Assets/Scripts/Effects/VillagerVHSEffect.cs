using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            mpb = new MaterialPropertyBlock();
            hatRenderer.GetPropertyBlock(mpb);

            for (int i = 0; i < xScanLines.Length; i++)
            {
                mpb.SetFloat("_xScanLine" + i, xScanLines[i] * Time.timeScale * Game.PastTimeScale);
            }

            mpb.SetFloat("_yScanLine", yScanLine * Time.timeScale * Game.PastTimeScale);
            mpb.SetFloat("_noiseStrength", noiseStrength);
            mpb.SetFloat("_ScanJitter", scanJitter);
            hatRenderer.SetPropertyBlock(mpb);
        }
        else
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
