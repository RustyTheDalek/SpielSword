using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDistort : MonoBehaviour {

    [Range(-1f, 1f)] public float displaceStrength =.01f;
    [Range(0, 1)] public float noiseStrength = .5f;

    public Material distort;


    float textureOffset;

    public float speed;

    float speedFlux = 1;

    public bool flux;

	// Use this for initialization
	void Start ()
    {
        textureOffset = Random.Range(0f, 1f);

        displaceStrength = Random.Range(-.01f, .01f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        textureOffset += Time.deltaTime * Time.timeScale * speed * speedFlux;

        if (textureOffset >= 1)
        {
            textureOffset = 0;
        }

        if (flux && Random.Range(0f, 1f) < .1f)
        {
            displaceStrength = Random.Range(-.01f, .01f);
            //speedFlux = Random.Range(0.75f, 1f);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        distort.SetFloat("_DisplaceStrength", displaceStrength);
        distort.SetFloat("_Offset", textureOffset);
        distort.SetFloat("_NoiseStrength", noiseStrength);
        //distort.mainTextureOffset += Vector2.up * textureOffset;
        Graphics.Blit(source, destination, distort);
    }
}
