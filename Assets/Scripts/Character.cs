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
    /// The direction for movement
    /// Left = -1
    /// None =  0
    /// Right=  1
    /// </summary>
    public Vector2 moveDir;

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

    private PlatformerCharacter2D m_Character;

    protected float health = 1;

    #endregion

    #region Private Variables

    private bool died = false;

    #endregion

    protected virtual void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();

        CreateHashtable();
    }

    protected virtual void CreateHashtable()
    {
        animData = new Hashtable
        {
            { "Move", new Vector2(0,0) },
            { "Dead", false },
            { "MeleeAttack", false },
            { "RangedAttack", false }
        };
    }

    public virtual void Update()
    {
        animData["Dead"] = !Alive;

        if(!Alive && !died)
        {
            died = true;
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        GetComponent<TimeObject>().tObjectState = TimeObjectState.PresentDead;

        animData["Move"] = Vector2.zero;

    }

    public virtual void FixedUpdate()
    {
        animData["Move"] = moveDir;
        m_Character.Move(animData);
    }
}
