using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScanLineEffect : MonoBehaviour
{
    public float[] xScanLines = new float[4];

    [Range(0, 1)] public float scanJitter = .695f;
    [Range(0, 1)] public float noiseStrength = 1f;

    public float xScanSpeed;

    private Material scanLine;

    // Creates a private material used to the effect
    void Awake()
    {
        scanLine = new Material(Shader.Find("ScreenEffects/ScanLines"));

        xScanSpeed = Random.Range(0.01f, .2f);
    }

    void OnEnable()
    {
        UpdateScanLines(true);
    }

    void OnDisable()
    {
        UpdateScanLines(false);
    }

    void Update()
    {
        UpdateScanLines(true);
    }

    void UpdateScanLines(bool vhs)
    {
        if (vhs)
        {

            for (int i = 0; i < xScanLines.Length; i++)
            {
                xScanLines[i] -= Time.deltaTime * xScanSpeed * (int)Game.timeState * Random.Range(1, i + 1);
                xScanLines[i] = XScanLineLogic(xScanLines[i]);
            }
        }
        else
        {
            //for (int i = 0; i < xScanLines.Length; i++)
            //{
            //    mpb.SetFloat("_xScanLine" + i, 0);
            //}
            //mpb.SetFloat("_yScanLine", 0);
            //mpb.SetFloat("_noiseStrength", 0);
            //mpb.SetFloat("_ScanJitter", 0);
            //sRenderer.SetPropertyBlock(mpb);
        }
    }

    float XScanLineLogic(float val)
    {
        if (val <= 0)
        {
            return 1;
        }
        else if (val >= 1)
        {
            return 0;
        }
        else
        {
            return val;
        }
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        for (int i = 0; i < xScanLines.Length; i++)
        {
            scanLine.SetFloat("_xScanLine" + i, xScanLines[i]);
        }
        scanLine.SetFloat("_ScanJitter", scanJitter);
        scanLine.SetFloat("_noiseStrength", noiseStrength);
        Graphics.Blit(source, destination, scanLine);
    }
}
