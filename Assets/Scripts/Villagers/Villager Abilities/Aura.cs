using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    public float health = 4;

    public bool comboUsed = false;

    public delegate void AuraEvent();
    public event AuraEvent OnEnterAuraEvent;

    GameObject creator;

    //public AuraTimeObject m_ATimeObject;
    public SpriteRenderer m_Sprite;
    public Collider2D m_Coll;

    protected virtual void Awake()
    {
        creator = GetComponentInParent<Villager>().gameObject;
    }

    public void SetAura(bool active)
    {
        if (m_Coll.enabled != active)
        {
            m_Coll.enabled = active;
        }
    }

    public void Detach()
    {
        transform.SetParent(null);
        //m_ATimeObject.enabled = true;
    }

    protected virtual void OnEnterAura(Villager villager)
    {
        Debug.Log("Entered " + name);

        if (villager.gameObject != creator.gameObject)
        {
            if (!comboUsed)
            {
                comboUsed = true;

                if (OnEnterAuraEvent != null)
                {
                    OnEnterAuraEvent();
                }
                else
                {
                    Debug.LogWarning("No event tied");
                }
            }
        }
    }

    protected virtual void OnExitAura(Villager villager)
    {
        Debug.Log(villager.name + ": Left " + name);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (health > 0 && coll.GetComponentInParent<Villager>())
        {
            Villager temp = coll.GetComponentInParent<Villager>();

            if (temp.CurrentVillager)
            {
                OnEnterAura(temp);
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponentInParent<Villager>())
        {
            Villager temp = coll.GetComponentInParent<Villager>();

            if (temp.CurrentVillager && health >= 0)
            {
                OnEnterAura(temp);
            }
            else
            {
                OnExitAura(temp);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponentInParent<Villager>())
        {
            Villager temp = coll.GetComponentInParent<Villager>();

            if (temp.CurrentVillager)
            {
                OnExitAura(temp);
            }
        }
    }

}
