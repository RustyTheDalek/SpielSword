using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VHSEffect : MonoBehaviour {

    private SpriteRenderer sRenderer;

    public float[] xScanLines = new float[2]; 
        
    public float yScanLine;

    [Range(-1, 1)] public float noiseStrength = 1;
    [Range(0, 1)] public float scanJitter = .695f;


    public float xScanSpeed, yScanSpeed;

    public void Start()
    {
        xScanSpeed = Random.Range(0.01f, .2f);
        yScanSpeed = Random.Range(0.01f, .2f);
    }

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

            yScanLine += Time.deltaTime * yScanSpeed * (int)Game.timeState;

            for (int i = 0; i < xScanLines.Length; i++)
            {
                xScanLines[i] -= Time.deltaTime * xScanSpeed * (int)Game.timeState * Random.Range(1, i+1);
                xScanLines[i] = XScanLineLogic(xScanLines[i]);
                mpb.SetFloat("_xScanLine" + i,  xScanLines[i] * Time.timeScale * Game.PastTimeScale);
            }

            if (yScanLine >= .75f || yScanLine <= 0)
            {
                yScanSpeed *= -1 * Random.Range(0.1f, 1);
            }

            //yScanLine = yScanLine % 1;

            mpb.SetFloat("_yScanLine", yScanLine * Time.timeScale * Game.PastTimeScale);
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
            mpb.SetFloat("_ScanJitter", 0);
            sRenderer.SetPropertyBlock(mpb);
        }
    }

    float XScanLineLogic(float val)
    {
        if (val <= 0 /*|| Random.value < 0.01*/)
        {
            return Random.value;
        }
        else
        {
            return val;
        }
    }
}
