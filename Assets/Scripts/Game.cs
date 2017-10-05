﻿using UnityEngine;
using System.Collections;

public static class Game
{
    public static TimeState timeState = TimeState.Forward;

    /// <summary>
    /// Custom Time Variable for past Villagers
    /// </summary>
    public static int t = 0;

    /// <summary>
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public static float longestTime = 0;

    /// <summary>
    /// Invcinibilty mode for testing
    /// </summary>
    public static bool GodMode = false;

    /// <summary>
    /// Whether the Boss is currently attackble by the Player 
    /// </summary>
    public static bool skippingStage;

    /// <summary>
    /// If the Boss is attackable
    /// </summary>
    public static bool attackable = true;

    public static bool debugText = false;

    /// <summary>
    /// Scaling for fastforward of boss and past objects
    /// </summary>
    public static float PastTimeScale = 1;

    public static int GameScale
    {
        get
        {
            return (int)Game.timeState * (int)Time.timeScale * (int)PastTimeScale;
        }
    }
}
