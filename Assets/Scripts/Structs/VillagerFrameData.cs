﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Frame Data for Villagers contains info relating to animation
/// </summary>
public class VillagerFrameData : FrameData
{

    public float    move,
                    health;

    public bool     meleeAttack,
                    rangedAttack,
                    meleeAttackEnd,
                    rangedAttackEnd,
                    special,
                    canSpecial,
                    specialEnd,
                    dead,
                    deathEnd;

    public string   spriteName;

    public Vector3  hatPos,
                    scale;

}