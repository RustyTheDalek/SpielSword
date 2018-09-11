﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
/// <summary>
/// Logs Game time info to attached Text object
/// Created on : Ian Jones      - 19/03/17
/// Updated by : Ian Jones      - 13/04/18
/// </summary>
public class GameTime : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = "Time: " + TimeObjectManager.t +
            "\nTimeState: " + TimeObjectManager.timeState +
            "\nTimeScale: " + Time.timeScale +
            "\nStart T: " + TimeObjectManager.startT +
            "\nPrev T: " + TimeObjectManager.prevT +
            "\nDelta T : " + TimeObjectManager.DeltaT;

        text.enabled = DebugPnl.debugText;
    }
}
#endif