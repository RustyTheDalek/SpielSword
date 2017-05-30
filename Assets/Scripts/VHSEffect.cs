using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VHSEffect : MonoBehaviour {

    private SpriteRenderer sRenderer;

    public float[] xScanLines = new float[3]; 
        
    public float yScanLine;

    [Range(0, 1)] public float noiseStrength;
    [Range(0, 1)] public float scanJitter;


    public float xScanSpeed, yScanSpeed;

    void OnEnable()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        UpdateVHS(true);
    }

    void OnDisable()
    {
        UpdateVHS(false);
    }

    void Update()
    {
        UpdateVHS(true);
    }

    void UpdateVHS(bool vhs)
    {
        if (vhs)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sRenderer.GetPropertyBlock(mpb);

            yScanLine += Time.deltaTime * yScanSpeed;

            for(int i = 0; i < xScanLines.Length; i++)
            {
                xScanLines[i] -= Time.deltaTime * xScanSpeed * Random.Range(1, i);
                xScanLines[i] = xScanLineLogic(xScanLines[i]);
                mpb.SetFloat("_xScanLine" + i,  xScanLines[i]);
            }

            if (yScanLine >= .75f || yScanLine <= 0)
            {
                yScanSpeed *= -1;
            }

            //yScanLine = yScanLine % 1;

            mpb.SetFloat("_yScanLine", yScanLine);
            mpb.SetFloat("_noiseStrength", noiseStrength);
            mpb.SetFloat("_ScanJitter", scanJitter);
            sRenderer.SetPropertyBlock(mpb);
        }
        else
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sRenderer.GetPropertyBlock(mpb);
            for (int i = 0; i < xScanLines.Length; i++)
            {
                mpb.SetFloat("_xScanLine" + i, 0);
            }
            mpb.SetFloat("_yScanLine", 0);
            mpb.SetFloat("_noiseStrength", 0);
            sRenderer.SetPropertyBlock(mpb);
        }
    }

    float xScanLineLogic(float val)
    {
        if (val <= 0 || Random.value < 0.01)
        {
            return Random.value;
        }
        else
        {
            return val;
        }
    }
}
