using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource LM;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Changes the currently playing track
    /// </summary>
    /// <param name="music"> The new music track to change to</param>
    public void ChangeLM(AudioClip music)
    {
        if (LM.clip.name == music.name)
            return;

        LM.Stop();
        LM.clip = music;
        LM.Play();
    }
}
