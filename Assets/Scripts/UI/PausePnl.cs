using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for handling pausing of game
/// Created by      : Ian - 15/03/18
/// </summary>
public class PausePnl : MonoBehaviour {

    Animator anim;

    float dTimeScale;

    public bool paused;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;

            if (paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

        }
    }

    public void Pause()
    {
        if (anim)
            anim.SetTrigger("Open");
        else
            Debug.LogError("PausePnl not set :(");

        dTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        if (anim)
            anim.SetTrigger("Close");
        else
            Debug.LogError("PausePnl not set :");

        Time.timeScale = dTimeScale;
        dTimeScale = 0;
    }

    public void Quit()
    {
    }
}
