using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages UI relating to end-game slate
/// Created by : Ian Jones - 13/04/18
/// </summary>
public class EndGamePnl : MonoBehaviour {

    public Text Victory,
                StageReached, 
                CombosUsed, 
                TimeTaken, 
                VillagersUsed;

    Animator anim;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}

    public void OpenSlate(bool victory, int stageReached, int combosUsed, int totalLives)
    {

        Victory.text = victory == true ? "Victory!" : "Failure...";

        StageReached.text += stageReached;  Debug.Log("Stage Reached : " + stageReached);
        CombosUsed.text += combosUsed;      Debug.Log("Combos Used: " + combosUsed);
        //TODO: Change this to convert to appropriate time
        TimeTaken.text += (TimeObjectManager.t / 60).ToString("#.##") + "s";   Debug.Log("Time Taken: " + (TimeObjectManager.t / 6) + "s");
        VillagersUsed.text += totalLives;                   Debug.Log("Villagers Used: " + totalLives);

        if (anim)
            anim.SetTrigger("Open");
        else
            Debug.LogError("PausePnl not set :");

        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToVillage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Village");
    }
}
