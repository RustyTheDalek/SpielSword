using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common class that Villagers (Player) and Minions (Enemys) derive from
/// </summary>
public class Character : MonoBehaviour {

    #region Public Variables

    public AttackType attackType;

    /// <summary>
    /// The Left right direction for movement
    /// Left = -1
    /// None =  0
    /// Right=  1
    /// </summary>
    public float xDir;

    public bool Alive
    {
        get
        {
            return health > 0;
        }
    }

    public PlatformerAnimData pAnimData;

    #endregion

    #region Protected Variables

    protected PlatformerCharacter2D m_Platformer;

    #endregion

    #region Private Variables

    float health = 1;

    bool m_Jump;

    #endregion

    public virtual void Awake()
    {
        m_Platformer = GetComponent<PlatformerCharacter2D>();

        pAnimData = new PlatformerAnimData()
        {
            move = 0,

            jump = false,
            meleeAttack = false,
            rangedAttack = false,
            dead = false,
        };

    }

    public virtual void Update()
    {
        pAnimData.dead = !Alive;
    }

    public virtual void FixedUpdate()
    {
        m_Platformer.Move(pAnimData);
    }
}
