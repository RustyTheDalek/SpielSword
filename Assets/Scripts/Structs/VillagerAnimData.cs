using UnityEngine;
using System.Collections;

/// <summary>
/// Animation data to be sent to Platformer Character2D
/// </summary>
public struct VillagerAnimData
{
    public float move;
    public bool jump;
    public bool meleeAttack;
    public bool rangedAttack;
    public bool shieldSpecial;
    public bool canSpecial;
    public bool dead;
}
