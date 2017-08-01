using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenDistort : MonoBehaviour {

    [Range(-.5f, .5f)] public float displaceStrength = -0.033f;

    private Material distort;

    public Texture displaceTex;

	// Use this for initialization
	void Start ()
    {
        distort = new Material(Shader.Find("ScreenEffects/ScreenDistort"));
        distort.SetTexture("_DisplacementTex", displaceTex);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        distort.SetFloat("_DisplaceStrength", displaceStrength);
        Graphics.Blit(source, destination, distort);
    }
}
