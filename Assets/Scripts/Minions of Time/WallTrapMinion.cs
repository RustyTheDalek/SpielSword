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

    // Update is called once per frame
    void Update()
    {

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
}
