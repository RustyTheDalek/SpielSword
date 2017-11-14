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
    public int xDir;

    public bool Alive
    {
        get
        {
            return health > 0;
        }
    }

    public Hashtable animData;

    #endregion

    #region Protected Variables

    protected PlatformerCharacter2D m_Platformer;

    protected float health = 1;

    protected bool m_Jump;

    #endregion

    #region Private Variables

    #endregion

    public virtual void Awake()
    {
        m_Platformer = GetComponent<PlatformerCharacter2D>();

        CreateHashtable();
    }

    protected virtual void CreateHashtable()
    {
        animData = new Hashtable
        {
            { "Move", 0 },
            { "Dead", false },
            { "MeleeAttack", false },
            { "RangedAttack", false },
            { "Jump", false }
        };
    }

    public virtual void Update()
    {
        animData["Dead"] = !Alive;
    }

    public virtual void FixedUpdate()
    {
        m_Platformer.Move(animData);
    }
}
