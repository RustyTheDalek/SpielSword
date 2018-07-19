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

    protected Rigidbody2D m_Rigidbody2D;

    [SerializeField] protected float m_MoveForce = 10f;                  // Strength of force that moves Character

    #endregion

    #region Private Variables
    #endregion

    protected virtual void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Move(Vector2 moveVector) { }
}
