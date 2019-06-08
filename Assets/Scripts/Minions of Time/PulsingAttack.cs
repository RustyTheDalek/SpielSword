using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for pulsing force that rock minion uses to push players back
/// </summary>
public class PulsingAttack : MonoBehaviour
{
    /// <summary>
    /// List of Objects being afffected by the Pulsing attack
    /// </summary>
    public List<Rigidbody2D> reactiveObjs = new List<Rigidbody2D>(5);

    [Header("Force Settigns")]

    public Vector2 pulseDirection;

    public AnimationCurve forceOverTime;

    public float timer = 0;

    private void OnEnable()
    {
        timer = 0;

        SetRangeConstraints(false);
    }

    private void OnDisable()
    {
        SetRangeConstraints(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <1)
            timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody2D obj in reactiveObjs)
        {
            obj.AddForce(new Vector2(Mathf.Sign(transform.PointTo(obj.transform).x), 
                pulseDirection.y) * forceOverTime.Evaluate(timer), ForceMode2D.Impulse);

            Debug.DrawRay(obj.transform.position, new Vector2(
                Mathf.Sign(transform.PointTo(obj.transform).x),
                pulseDirection.y), Color.red);
        }
    }

    /// <summary>
    /// Sets Rigidbody Constraints of the Ranged projectiles within Pulse minions 
    /// influence
    /// </summary>
    /// <param name="active"> 1 = Activating constraints. 0 = Deactivating constraints.</param>
    public void SetRangeConstraints(bool constrain)
    {
        foreach (Rigidbody2D rb in reactiveObjs)
        {
            if (LayerMask.LayerToName(rb.gameObject.layer) == "Weapon")
            {
                if (constrain)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb;

        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                Villager villager = collision.GetComponentInParent<Villager>();
                rb = villager.GetComponentInChildren<Rigidbody2D>();

                if (reactiveObjs.Count < reactiveObjs.Capacity && 
                    !reactiveObjs.Contains(rb) && villager.Alive)
                {
                    reactiveObjs.Add(rb);
                    villager.pData.velocityDampen = .5f;
                }

                break;

            case "Weapon":

                rb = collision.GetComponentInParent<Rigidbody2D>();

                if (rb != null)
                {
                    if (reactiveObjs.Count < reactiveObjs.Capacity &&
                    !reactiveObjs.Contains(rb))
                    {
                        reactiveObjs.Add(rb);
                    }
                }

                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb;

        switch (LayerMask.LayerToName(collision.gameObject.layer))
        {
            case "Villager":
            case "PastVillager":

                rb = collision.GetComponentInParent<Rigidbody2D>();

                if (rb != null)
                {
                    if (reactiveObjs.Contains(rb))
                    {
                        reactiveObjs.Remove(rb);
                        rb.GetComponentInParent<Villager>().pData.velocityDampen = 0;

                    }
                }

                break;

            case "Weapon":

                rb = collision.GetComponentInParent<Rigidbody2D>();

                if(rb != null)
                {
                    if(reactiveObjs.Contains(rb))
                    {
                        reactiveObjs.Remove(rb);
                        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    }
                }

            break;
        }
    }
}