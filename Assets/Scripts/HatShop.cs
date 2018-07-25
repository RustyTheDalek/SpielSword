using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShop : MonoBehaviour {

    public Hat[] hats;

    public HatDisplay hatdisplayPrefab;

    public RectTransform hatContent;

	// Use this for initialization
	void Start ()
    {
        hats = Resources.LoadAll<Hat>("Hats");

        foreach(Hat hat in hats)
        {
            HatDisplay newDispaly = Instantiate(hatdisplayPrefab, hatContent, false);
            newDispaly.LoadInfo(hat);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
