using UnityEngine;

public struct VillagerFrameData
{
    public int move;
    public float health;

    public bool meleeAttack,
                rangedAttack,
                meleeAttackEnd,
                rangedAttackEnd,
                special,
                canSpecial,
                specialEnd,
                dead,
                deathEnd,
                marty,
                unmarty;

    public string spriteName;

    public Vector3 hatPos,
                    scale;
}