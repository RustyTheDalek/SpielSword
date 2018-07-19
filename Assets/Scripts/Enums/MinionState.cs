/// <summary>
/// What Minion is currently doing
/// </summary>
public enum MinionState
{
    Patrolling,     //The typical state, just meandering in level
    ClosingIn,      //Found enemy and moving in to attack
    Attacking,      //In range and ready for attack
    Migrating,      //Simple moving from right to left of map
    Resting,        //Recovering from attack
}