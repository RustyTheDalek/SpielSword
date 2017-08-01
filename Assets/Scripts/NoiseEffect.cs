using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NoiseEffect : MonoBehaviour
{

    [Range(-1, 1)] public float noiseStrength = 1;

    private Material noise;

    // Creates a private material used to the effect
    void Awake()
    {
        noise = new Material(Shader.Find("ScreenEffects/ScreenNoise"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (noiseStrength == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        noise.SetFloat("_noiseStrength", noiseStrength);

        Graphics.Blit(source, destination, noise);
    }
}
