using UnityEngine;

public struct VillagerFrameData
{
    public float move,
                 health;

    public bool meleeAttack,
                rangedAttack,
                meleeAttackEnd,
                rangedAttackEnd,
                special,
                canSpecial,
                specialEnd,
                dead,
                deathEnd;

    public string spriteName;

    public Vector3 hatPos,
                    scale;
}