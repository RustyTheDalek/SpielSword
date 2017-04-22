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
    public bool meleeAttackEnd;
    public bool rangedAttack;
    public bool rangedAttackEnd;
    public bool playerSpecial;
    public bool canSpecial;
    public bool dead;
    public bool deathEnd;
    public bool playerSpecialIsTrigger;
}
