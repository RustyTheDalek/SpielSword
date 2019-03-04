using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanTotem : MonoBehaviour
{
    public float lifeTime = 15f;

    float timer;

    //TODO: Have list of villagers interacted with this to check for multiple combos if desired
    public bool comboUsed = false;

    [Header("References")]
    public SpriteRenderer m_Sprite;
    public Collider2D m_Collider;
    public GameObject creator;

    public delegate void TimeObjectEvent();
    public event TimeObjectEvent OnUsedTotem;

    private void Start()
    {
        timer = lifeTime;
    }


    private void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
        else
        {
            m_Sprite.enabled = false;
            m_Collider.enabled = false;
            enabled = false;
            GetComponent<TimeObject>().FinishTracking();
        }

        m_Sprite.color = m_Sprite.color.SetAlpha(timer / lifeTime);
    }

    public void Setup(VillagerManager villagerManager)
    {
        OnUsedTotem += villagerManager.IncCombosUsed;
    }

    public void Unsubscribe(VillagerManager villagerManager)
    {
        OnUsedTotem -= villagerManager.IncCombosUsed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (creator.gameObject != collision.gameObject)
        {
            if (!comboUsed)
            {
                if (OnUsedTotem != null)
                    OnUsedTotem();
            }
        }
    }
}
