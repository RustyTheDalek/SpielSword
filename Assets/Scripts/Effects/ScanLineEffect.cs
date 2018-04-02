using UnityEngine;

/// <summary>
/// Manages Scan Line effect on screen
/// Created by : Ian Jones      - 01/08/17
/// Updated by : Ian Jones      - 02/04/18
/// </summary>
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

    void UpdateScanLines(bool active)
    {
        if (active)
        {

            for (int i = 0; i < xScanLines.Length; i++)
            {
                xScanLines[i] -= Time.deltaTime * xScanSpeed * (int)Game.timeState * Random.Range(1, i + 1);
                xScanLines[i] = XScanLineLogic(xScanLines[i]);
            }
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
