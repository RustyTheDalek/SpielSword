using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages (No shit) the Game itself, handles logic relating to overall game e.g. 
/// handling the skipping of Boss stages when player causes a paradox
/// Created by      : Ian - 24/07/17
/// Last updated    : Ian - 24/07/17
/// </summary>
public class GameManager : MonoBehaviour {

    public BossManager  bossTemplate,
                        currentBoss;

    public VillagerManager vilManager;

    public TimeObjectManager timeManager;

    /// <summary>
    /// What kind of skipping stage technique are we using?
    /// </summary>
    enum SkipStageType
    {
        /// <summary>
        /// Simple speed up of boss
        /// </summary>
        FastForward,
        /// <summary>
        /// Spawns a 'punching bag' to give the player something to fight while the boss catches up.
        /// </summary>
        Punchbag,
        /// <summary>
        /// Wipes all the Villagers when the paradox is reached and boss begins at new wave.
        /// </summary>
        VillagerWipe
    };

    SkipStageType skipStageType = SkipStageType.FastForward;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (Game.timeState)
        {
            case TimeState.Forward:

                if (timeManager.newRoundReady)
                {
                    currentBoss.Reset();
                    vilManager.OnNewRound();

                    timeManager.newRoundReady = false;
                }

                if (!vilManager.activeVillager.Alive)
                {
                    Game.timeState = TimeState.Backward;
                    vilManager.OnVillagerDeath();
                    currentBoss.GetComponent<BossManager>().SetAnimators(false);
                    TimeObjectManager.SoftReset();
                }

                if (currentBoss.skippingStage)
                {
                    switch (skipStageType)
                    {
                        case SkipStageType.FastForward:

                            currentBoss.FastforwardSkip();
                            break;
                    }
                }
                else
                {
                    switch (skipStageType)
                    {
                        case SkipStageType.FastForward:

                            Game.PastTimeScale = 1;
                            break;
                    }
                }

                break;

            //case TimeState.Backward:

            //    if (timeManager.newRoundReady)
            //    {
            //        currentBoss.Reset();
            //        vilManager.OnNewRound();

            //        timeManager.newRoundReady = false;
            //    }
            //    break;
        }
	}
}
