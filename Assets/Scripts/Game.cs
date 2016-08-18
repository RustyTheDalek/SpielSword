using UnityEngine;
using System.Collections;

public static class Game
{
    public static TimeState timeState;

    /// <summary>
    /// Time Scale for Past Villagers
    ///  1 = Normal Time
    /// -1 = Reversed Time;
    /// </summary>
    public static float timeScale = 1;

    /// <summary>
    /// Custom Time Variable for past Villagers
    /// </summary>
    public static float t;

    /// <summary>
    /// Scaling for Time Helps speed up reversal
    /// </summary>
    public static float longestTime = 0;
}
