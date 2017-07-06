using UnityEngine;

/// <summary>
/// Base data for a object to be controlled by time
/// </summary>
public struct BaseFrameData
{
    public int timeStamp;

    public Vector3 m_Position;
    public Quaternion m_Rotation;

    public bool enabled;
}
