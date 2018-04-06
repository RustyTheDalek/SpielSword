using UnityEngine;

/// <summary>
/// Tracks Villager related data for playback
/// Created by : Ian Jones - 19/03/17 
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public struct VillagerFrameData
{
    public int move;
    public float health;

    public bool meleeAttack,
                rangedAttack,
                special,
                canSpecial,
                dead,
                marty;

    /// <summary>
    /// What Sprite was active in that frame, cheaper than tracking the actual sprite
    /// </summary>
    public string spriteName;

    public Vector3 hatPos,
                    scale;
}