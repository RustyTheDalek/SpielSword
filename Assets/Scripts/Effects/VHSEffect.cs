using UnityEngine;

/// <summary>
/// Manages noise effect on Sprite
/// Created by : Ian Jones      - 30/05/17
/// Updated by : Ian Jones      - 02/04/18
/// </summary>
[ExecuteInEditMode]
public class VHSEffect : MonoBehaviour {

    private SpriteRenderer sRenderer;

    public float[] xScanLines = new float[2]; 
        
    public float yScanLine;

    [Range(-1, 1)] public float noiseStrength = 1;
    [Range(0, 1)] public float scanJitter = .695f;

    public MaterialPropertyBlock mpb;

    public float xScanSpeed, yScanSpeed;

    public virtual void Start()
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

    public virtual void UpdateVHS(bool active)
    {
        if (active)
        {
            mpb = new MaterialPropertyBlock();
            sRenderer.GetPropertyBlock(mpb);

            //Moves the "Smudge effect"
            yScanLine += Time.deltaTime * yScanSpeed * (int)TimeObjectManager.timeState;

            //Moves all the scan lines around
            for (int i = 0; i < xScanLines.Length; i++)
            {
                xScanLines[i] -= Time.deltaTime * xScanSpeed * (int)TimeObjectManager.timeState * Random.Range(1, i+1);
                xScanLines[i] = XScanLineLogic(xScanLines[i]);
                mpb.SetFloat("_xScanLine" + i,  xScanLines[i] * (int)Time.timeScale);
            }

            //Clamps the smudge effect
            if (yScanLine >= .75f || yScanLine <= 0)
            {
                yScanSpeed *= -1 * Random.Range(0.1f, 1);
            }

            mpb.SetFloat("_yScanLine", yScanLine * (int)Time.timeScale);
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

    //This causes Lines to move randomly up when they reach the bottom
    float XScanLineLogic(float val)
    {
        if (val <= 0 )
        {
            return Random.value;
        }
        else
        {
            return val;
        }
    }
}
