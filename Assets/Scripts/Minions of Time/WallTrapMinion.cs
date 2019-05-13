using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallTrapMinion : FlightMinion
{
    [Header("Time between eyes dropping off to spawn minions")]
    public float minionSpawnInterval = 1.5f;

    public float timer = 0;

    Queue<SummonEye> summonEyes;

    SummonEye currentEye;

    protected override void Awake()
    {
        base.Awake();

        summonEyes = new Queue<SummonEye>(GetComponentsInChildren<SummonEye>());
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
        timer += Time.deltaTime;

        if(timer >= minionSpawnInterval)
        {
            timer = 0;
            Debug.Log("Spawning Minion");
            SpawnMinion();
        }
    }

    void SpawnMinion()
    {
        if (summonEyes.Count > 0)
        {
            currentEye = summonEyes.Dequeue();
            currentEye.transform.SetParent(null);
            currentEye.enabled = true;
        }
    }
}
