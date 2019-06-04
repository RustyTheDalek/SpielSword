using UnityEngine;
using System.Collections;

/// <summary>
/// Base Controller for 2D characters
/// </summary>
public class PlatformerCharacter2D : MonoBehaviour
{
    #region Public Variables
    #endregion

    #region Protected Variables

    public Rigidbody2D m_Rigidbody2D;

    [SerializeField] protected float m_MoveForce = 10f;
    [SerializeField] protected float m_MaxVelocity = 7f;

    #endregion

    #region Private Variables
    #endregion

    public virtual void Move(PlatformerData pData)
    {

        m_MaxVelocity = pData.maxVelocity;

        m_Rigidbody2D.AddForce(pData.moveDir * m_MoveForce, ForceMode2D.Impulse);
        m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, m_MaxVelocity);
    }
}
