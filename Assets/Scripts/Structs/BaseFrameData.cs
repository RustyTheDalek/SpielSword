using UnityEngine;

/// <summary>
/// Base data for a object to be controlled by time
/// Tracks everything you'd find in a basic GameObject i.e. transform variables
/// and whether the gameObject is enabled.
/// Created by : Ian Jones - 06/07/17
/// Updated by : Ian Jones - 06/04/18
/// </summary>
public struct TransformFrameData
{
    public int timeStamp;

    public Vector3 m_Position;
    public Quaternion m_Rotation;
    public Vector3 m_Scale;

    public bool enabled;
}
