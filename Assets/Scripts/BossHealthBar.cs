using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages health Bar fill and state
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    Image healthBar;

    public Sprite   standardSprite,
                    invincibleSprite;

    HealthBarState oldHBState = HealthBarState.Standard;

    void Awake()
    {
        healthBar = GetComponentInChildren<Image>();
        standardSprite = healthBar.sprite;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.fillAmount = BossManager.health/100f;
	}

    /// <summary>
    /// Sets the sprite of the Health bar based on the Boss' state
    /// </summary>
    /// <param name="newState"> New state to be passed</param>
    public void SetHealthBar(HealthBarState newState)
    {
        if (newState != oldHBState)
        {
            switch (newState)
            {
                case HealthBarState.Standard:

                    healthBar.sprite = standardSprite;
                    break;

                case HealthBarState.Invincible:

                    healthBar.sprite = invincibleSprite;
                    break;
            }

            oldHBState = newState;
        }
    }
}
