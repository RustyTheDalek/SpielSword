using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMinion : GroundMinion {

    #region Public Variables

    [Header("Rock throwing settings")]
    public int m_ProjectilesToSpawn = 3;
    public int m_RockSpeed = 10;
    public float m_GravityStrength = 9.81f;
    #endregion

    #region Protected variables

    protected readonly int m_HashAttackLeft = Animator.StringToHash("AttackLeft");
    protected readonly int m_HashAttackRight = Animator.StringToHash("AttackRight");
    protected readonly int m_HashAttackThrowRocks = Animator.StringToHash("ThrowRocks");

    protected List<MinionHitAttack> activeProjectiles = new List<MinionHitAttack>();

    #endregion

    protected override void StartAttack()
    {
        state = MinionState.Attacking;

        prevDir = moveDir;

        if(moveDir.y > .75f || moveDir.y < -.75f)
        {
            Debug.Log("They're not on the same plan as me, going to yeet some rocks instead");

            m_Animator.SetTrigger(m_HashAttackThrowRocks);
        }
        else if (moveDir.x < 1)
        {
            Debug.Log("Attacking to Left");
            m_Animator.SetTrigger(m_HashAttackLeft);
        }
        else
        {
            Debug.Log("Attacking to Right");
            m_Animator.SetTrigger(m_HashAttackRight);
        }
    }

    protected override void StartAttack(AttackType attackType)
    {
        state = MinionState.Attacking;

        prevDir = moveDir;

        if (Mathf.Abs(moveDir.y) > .5f)
        {
            Debug.Log("They're not on the same plan as me, going to yeet some rocks instead");

            m_Animator.SetTrigger(m_HashAttackThrowRocks);
        }
        else if (moveDir.x < 1)
        {
            Debug.Log("Attacking to Left");
            m_Animator.SetTrigger(m_HashAttackLeft);
        }
        else
        {
            Debug.Log("Attacking to Right");
            m_Animator.SetTrigger(m_HashAttackRight);
        }
    }

    public void FireProjectiles(BallisticMotion objToSpawn)
    {
        Vector3 targetPos = closestVillager.Rigidbody.transform.position;
        Vector3 diff = targetPos - rangedSpawn.position;
        Vector3 diffGround = new Vector3(diff.x, 0f, diff.z);

        Vector3[] solutions = new Vector3[2];
        int numSolutions;
        BallisticMotion proj;

        for (int i = 0; i < m_ProjectilesToSpawn; i++)
        {
            Vector3 posVariance = new Vector3(
                Random.Range(-.75f, .75f),
                Random.Range(-.75f, .75f), 0);

            numSolutions = fts.solve_ballistic_arc(rangedSpawn.position + posVariance, m_RockSpeed + Random.Range(-2, 2),
                targetPos, m_GravityStrength, out solutions[0], out solutions[1]);

            proj = objToSpawn.Spawn(null, rangedSpawn.position);
            proj.Initialize(rangedSpawn.position, m_GravityStrength);

            proj.GetComponent<MinionHitAttack>().OnAttack += OnHit;
            activeProjectiles.Add(proj.GetComponent<MinionHitAttack>());

            var impulse = solutions[1];

            proj.AddImpulse(impulse);

            if (targetPos.x > transform.position.x)
                proj.GetComponent<ConstantRotation>().m_RotationSpeed *= -1;
        }
    }

    public void OnHit(MinionHitAttack projectile, bool hitPlayer)
    {
        if(hitPlayer)
            m_Animator.SetTrigger(m_HashCelebrateParam);

        projectile.OnAttack -= OnHit;

        activeProjectiles.Remove(projectile);

    }

    private void OnApplicationQuit()
    {
        foreach(MinionHitAttack hitAttack in activeProjectiles)
        {
            hitAttack.OnAttack -= OnHit;
        }
    }
}
