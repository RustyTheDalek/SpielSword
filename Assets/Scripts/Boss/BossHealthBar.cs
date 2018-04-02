using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages health Bar fill and state
/// Created by : Ian Jones      - 18/08/16
/// Updated by : Ian Jones      - 02/04/18
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
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.fillAmount = BossManager.health/BossManager.MAXHEALTH;
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
