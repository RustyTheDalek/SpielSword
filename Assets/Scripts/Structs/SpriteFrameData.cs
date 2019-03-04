using UnityEngine;

/// <summary>
/// Tracks sprite information
/// Created by : Ian Jones - 06/07/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public struct SpriteFrameData
{
    public bool enabled;
    public string sprite;
    public Color color;
    public bool flipX, flipY;
    public SpriteMaskInteraction maskInteraction;
}