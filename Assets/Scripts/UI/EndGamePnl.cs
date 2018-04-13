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

    public Text StageReached, 
                CombosUsed, 
                TimeTaken, 
                VillagersUsed;

    Animator anim;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();

        BossManager.OnBossDeath += OpenSlate;
	}

    public void OpenSlate()
    {

        StageReached.text += Game.stageReached;             Debug.Log("Stage Reached : " + Game.stageReached);
        CombosUsed.text += Game.combosUsed;                 Debug.Log("Combos Used: " + Game.combosUsed);
        //TODO: Change this to convert to appropriate time
        TimeTaken.text += Game.t / 60 + "s";                Debug.Log("Time Taken: " + (Game.t / 6) + "s");
        VillagersUsed.text += VillagerManager.totalLives;   Debug.Log("Villagers Used: " + VillagerManager.totalLives);

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
