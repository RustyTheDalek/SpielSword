using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallTrapMinion : FlightMinion
{
    [Header("Time between eyes dropping off to spawn minions")]
    public float minionSpawnInterval = 1.5f;

    public float timer = 0;

    Queue<SummonEye> attachedSummonEyes;
    List<SummonEye> detachedSummonEyes = new List<SummonEye>();

    SummonEye currentEye;

    protected override void Awake()
    {
        base.Awake();

        attachedSummonEyes = new Queue<SummonEye>(GetComponentsInChildren<SummonEye>());
    }

    public override void OnEnable()
    {
        WallTrapperSpawnSMB.Initialise(m_Animator, this);
        MinionPatrolSMB.Initialise(m_Animator, this);
    }


    public void MinionSpawning()
    {
        if (attachedSummonEyes.Count > 0)
        {
            timer += Time.deltaTime;

            if (timer >= minionSpawnInterval)
            {
                timer = 0;
                Debug.Log("Spawning Minion");
                SpawnMinion();
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void CheckDetachedEyes()
    {
        foreach (SummonEye eye in detachedSummonEyes.ToList())
        {
            if (eye.eyeState == SummonEyeState.None)
            {
                attachedSummonEyes.Enqueue(eye);
                detachedSummonEyes.Remove(eye);
                eye.enabled = false;
            }
        }
    }

    void SpawnMinion()
    {
        if (attachedSummonEyes.Count > 0)
        {
            currentEye = attachedSummonEyes.Dequeue();
            currentEye.transform.SetParent(null, true);
            currentEye.enabled = true;
            detachedSummonEyes.Add(currentEye);
        }
    }

    protected override void OnDeath(Vector2 attackDirection)
    {
        base.OnDeath(attackDirection);

        if (minionGibs)
        {
            //Any eyes that are firing or returning fall gib like the the rest.
            foreach (SummonEye summonEye in detachedSummonEyes)
            {
                summonEye.enabled = false;

                switch (summonEye.eyeState)
                {
                    case SummonEyeState.Firing:
                    case SummonEyeState.Returning:

                        Debug.Log("Throwing Eye off");
                        summonEye.Throw();
                        break;

                    case SummonEyeState.None:

                        summonEye.DisablePhysics();
                        break;

                    case SummonEyeState.Summoned:

                        Debug.Log("Killing attached Minion");
                        summonEye.KillMinion();
                        break;
                }
            }

            while(attachedSummonEyes.Count > 0)
            {
                var summonEye = attachedSummonEyes.Dequeue();
                summonEye.enabled = false;
                summonEye.DisablePhysics();
            }
        }

        //Force kill any summoned minions
        //Animates the walls failing in some way
    }
}
