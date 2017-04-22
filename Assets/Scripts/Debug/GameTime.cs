using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour {

    Text text;
	// Use this for initialization
	void Start () {

        text = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {

        text.text = "Time: " + Game.t +
            "\nTimeState: " + Game.timeState;
	}
}
