/// <summary>
/// What kind of skipping stage technique are we using?
/// </summary>
public enum SkipStageType
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