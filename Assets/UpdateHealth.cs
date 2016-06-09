using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Image>().fillAmount = Golem.health/100f;
	}
}
