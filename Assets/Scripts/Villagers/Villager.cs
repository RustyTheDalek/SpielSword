using System;
using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets._2D;

/// <summary>
/// Class for controller Villager
/// </summary>
[RequireComponent(typeof(PlatformerCharacter2D))]
public abstract class Villager : MonoBehaviour
{
    public VillagerState villagerState = VillagerState.Waiting;
    public AttackType villagerAttackType = AttackType.Melee;

    Transform rangedTrans;

    static GameObject rangedPrefab;

    float rangedStrength = 25;

    public PlatformerCharacter2D m_Character;
    private bool m_Jump;
    public float xDir;

    public float health = 1;

    public Vector3 startingPos;

    //Target X position for Villager to aim for when they're waiting in queue
    public float targetX;

    public  VillagerAnimData animData;

    /// <summary>
    /// Whether the Villager is advancing in the queue
    /// </summary>
    public bool advancing;

    public bool alive
    {
        get
        {
            return health > 0;
        }
    }

    //Whether it's currently in control by the player
    public bool activePlayer
    {
        get
        {
            return villagerState == VillagerState.CurrentVillager;
        }
    }

    public ParticleSystem deathEffect;

    public CircleCollider2D melee;

    public CircleCollider2D[] PlayerCollisions;

    #region pastVillager variables

    public List<Action> actions = new List<Action>();
    public Action currentAction;

    /// <summary>
    /// Special time in which Villagers "Finish" Dying.
    /// </summary>
    public int reverseDeathTimeStamp = 0;

    #endregion

    public virtual void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        deathEffect = GetComponentInChildren<ParticleSystem>();
        startingPos = transform.position;

        rangedTrans = GameObject.Find(this.name + "/RangedTransform").transform;

        //villagerState = VillagerState.Waiting;

        //TO-DO: FIX THIS TRASH
        melee = GetComponentInChildren<MeleeAttack>().GetComponentInChildren<CircleCollider2D>();

        PlayerCollisions = GetComponents<CircleCollider2D>();

        animData.canSpecial = true;
        villagerState = VillagerState.Waiting;

        if (!rangedPrefab)
        {
            rangedPrefab = (GameObject)Resources.Load("Range");
        }
    }

    public virtual void Update()
    {
        switch(villagerState)
        {
            case VillagerState.CurrentVillager:   

                //Get PlayerMovement
                xDir = 0;
                xDir = ((Input.GetKey(KeyCode.D)) ? 1 : xDir);
                xDir = ((Input.GetKey(KeyCode.A)) ? -1 : xDir);

                switch (villagerAttackType)
                {
                    case AttackType.Melee:
                        animData.meleeAttack = Input.GetKey(KeyCode.DownArrow);
                        CanAttack(animData.meleeAttack);
                        break;

                    case AttackType.Ranged:
                        animData.rangedAttack = Input.GetKey(KeyCode.DownArrow);
                        CanAttack(animData.rangedAttack);
                        break;    
                }

                OnSpecial(Input.GetKey(KeyCode.LeftArrow));

                if (!m_Jump)
                {
                    // Read the jump input in Update so button presses aren't missed.
                    m_Jump = Input.GetKeyDown(KeyCode.Space);
                }

                RecordFrame();

                break;

            case VillagerState.Waiting:

                //Wander/AI Code
                if (Mathf.Abs(transform.localPosition.x - targetX) > .5f)
                {
                    xDir = Mathf.Clamp01(targetX - transform.localPosition.x);
                }
                else
                {
                    advancing = false;
                }

                break;

            case VillagerState.PastVillager:

                if (actions != null)
                {
                    //So long as T is within Range
                    if (Game.t < actions.Count && Game.t >= 0)
                    {
                        //Set new position and adjust for Time Scale
                        GetComponent<Rigidbody2D>().transform.position = actions[Game.t].pos;
                        animData.move = actions[Game.t].move;
                        animData.jump = actions[Game.t].jump;
                        switch (villagerAttackType)
                        {
                            case AttackType.Melee:
                                animData.meleeAttack = actions[Game.t].meleeAttack;
                                break;

                            case AttackType.Ranged:
                                animData.rangedAttack = actions[Game.t].rangedAttack;
                                break;
                        }
                        animData.playerSpecial = actions[Game.t].special;
                        animData.canSpecial = actions[Game.t].canSpecial;
                        animData.dead = actions[Game.t].dead;
                    }
                    else if (Game.t == actions.Count)
                    {
                        animData.move = 0;
                        animData.jump = false;
                        animData.meleeAttack = false;
                        animData.dead = false;
                    }

                    if (Game.timeState == TimeState.Backward)
                    {
                        if (reverseDeathTimeStamp != 0 &&
                            reverseDeathTimeStamp == Game.t)
                        {
                            //Debug.Break();
                            Debug.Log("Villager Un-Dying");
                            GetComponent<Animator>().SetTrigger("ExitDeath");
                        }
                    }

                }

                break;
        }
    }

    public void CanAttack(bool attack)
    {
        if (!attack)
        {
            GetComponent<Animator>().SetBool("CanAttack", true);
        }
    }

    public void RecordFrame()
    {
        currentAction = new Action();

        currentAction.timeStamp = Time.timeSinceLevelLoad;
        currentAction.pos = transform.position;
        currentAction.move = xDir;
        switch (villagerAttackType)
        {
            case AttackType.Melee:
                currentAction.meleeAttack = Input.GetKey(KeyCode.DownArrow);
                break;

            case AttackType.Ranged:
                currentAction.rangedAttack = Input.GetKey(KeyCode.DownArrow);
                break;
        }
        currentAction.health = health;
        currentAction.special = Input.GetKey(KeyCode.LeftArrow);
        currentAction.canSpecial = animData.canSpecial;

        actions.Add(currentAction);
    }

    public void actionsSetup(List<Action> _Actions)
    {
        actions = new List<Action>();
        actions.AddRange(_Actions);
    }

    private void FixedUpdate()
    {
        switch (villagerState)
        {
            case VillagerState.CurrentVillager:

                animData.move = xDir;
                animData.jump = m_Jump;

                //animData.dead = !alive;

                m_Character.Move(animData);
                m_Jump = false;
                break;

            case VillagerState.Waiting:

                animData.move = xDir;
                animData.dead = false;
                animData.jump = false;
                animData.meleeAttack = false;
                m_Character.Move(animData);
                break;

            case VillagerState.PastVillager:

                if (actions != null)
                {
                    if (Game.t < actions.Count &&
                        Game.t >= 0)
                    {
                        m_Character.Move(animData);
                    }
                    else if (Game.t == actions.Count)
                    {
                        m_Character.Move(animData);
                    }
                }
                break;
        }
    }
    
    /// <summary>
    /// Kills player
    /// </summary>
    public void Kill()
    {
        health = 0;
    }

    /// <summary>
    /// Sets target for Villager to aim for in the x axis
    /// </summary>
    /// <param name="xPos">Target X Position</param>
    public void SetTarget(float xPos)
    {
        targetX = xPos;
        advancing = true;
    }

    public void SetTrigger(bool active)
    {
        PlayerCollisions[0].isTrigger = active;
        PlayerCollisions[1].isTrigger = active;
    }

    public abstract void OnSpecial(bool playerSpecial);

    public virtual void OnHit()
    {
        health--;
    }

    public virtual void OnPastHit(Collider2D collider)
    {
        if (collider.GetComponent<BossAttack>() && !animData.dead &&
            Game.timeState == TimeState.Forward)
        {
            Debug.Log("Past Villager Hit By Boss Attack");
            animData.dead = true;
            m_Character.Move(animData);

            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (villagerState == VillagerState.PastVillager)
        {
            OnPastHit(collider);
        }
    }

    public void FireProjectile()
    {
        Debug.Log("Ranged Attack"); 
        GameObject attack = Instantiate(rangedPrefab, rangedTrans.position, Quaternion.identity) as GameObject;

        float direction = rangedTrans.position.x - transform.position.x;

        attack.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(direction),0) * rangedStrength, ForceMode2D.Impulse);
    }

    public void CannotAttack()
    {
        GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = false;
    }

    public void CanAttack()
    {
        GetComponentInChildren<MeleeAttack>().GetComponent<CircleCollider2D>().enabled = true;
    }
}

