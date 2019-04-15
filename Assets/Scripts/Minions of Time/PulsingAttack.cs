using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for pulsing force that rock minion uses to push players back
/// </summary>
public class PulsingAttack : MonoBehaviour
{
    public List<Villager> villagers = new List<Villager>(5);

    [Header("Force Settigns")]

    public Vector2 pulseDirection;

    public AnimationCurve forceOverTime;

    public float timer = 0;

    private void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <1)
            timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        foreach(Villager villager in villagers)
        {
            villager.GetComponentInChildren<Rigidbody2D>().AddForce(
                new Vector2(Mathf.Sign(villager.Rigidbody.transform.position.x - transform.position.x), 
                pulseDirection.y) * forceOverTime.Evaluate(timer), ForceMode2D.Impulse);

            Debug.DrawRay(villager.Rigidbody.transform.position, new Vector2(Mathf.Sign(villager.Rigidbody.transform.position.x - transform.position.x),
                pulseDirection.y), Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                Villager villager = collision.GetComponentInParent<Villager>();

                if (villagers.Count < villagers.Capacity &&
                !villagers.Contains(villager) &&
                villager.Alive) // don't want them to attack dead villagers
                {
                    villagers.Add(villager);
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Villager":
                case "PastVillager":

                    Villager villager = collision.GetComponentInParent<Villager>();

                    if (villagers.Contains(villager))
                    {
                        villagers.Remove(villager);
                    }
                break;
            }
        }
}
