using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

    public static float health = 100;

    public bool alive
    {
        get
        {
            return health > 0;
        }
    }
    public float currentHealth;
    public Image healthBar;

    public Animator leftArm, rightArm;

    Timer LAttack1, RAttack1,
          LAttack2, RAttack2;

    int totalSweeps = 3, currentSweeps = 0,
        totalSlams = 4, currentSlams = 0;

    List<float> healthRecord;

    public List<SpriteRenderer> bossParts;

    public List<Sprite> headStages, chestStages, lArmStages, rArmStages, c2, c3, c4;
	// Use this for initialization
	void Start () {

        LAttack1 = gameObject.AddComponent<Timer>();
        LAttack1.Setup("LeftHandAttack", 2, true);

        LAttack1.StartTimer();

        RAttack1 = gameObject.AddComponent<Timer>();
        RAttack1.Setup("RightHandAttack", 2.5f, true);
        RAttack1.enabled = false;

        LAttack2 = gameObject.AddComponent<Timer>();
        LAttack2.Setup("LHandSlam", 1f, true);

        RAttack2 = gameObject.AddComponent<Timer>();
        RAttack2.Setup("RHandSlam", 1f, true);


    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            health = 49;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            health = 0;
        }


        if (health < 50 && health > 0)
        {
            bossParts[0].sprite = headStages[0];
            bossParts[1].sprite = chestStages[0];
            bossParts[2].sprite = c2[0];      
            bossParts[3].sprite = c3[0];
            bossParts[4].sprite = c4[0];
            bossParts[5].sprite = rArmStages[0];
            bossParts[6].sprite = lArmStages[0];
        }
        else if(health <= 0)
        {
            bossParts[0].sprite = headStages[1];
            bossParts[1].sprite = chestStages[1];
            bossParts[2].sprite = c2[1];
            bossParts[3].sprite = c3[1];
            bossParts[4].sprite = c4[1];
            bossParts[5].sprite = rArmStages[1];
            bossParts[6].sprite = lArmStages[1];
        }
        currentHealth = health;
        if (alive)
        {
            if (currentSweeps <= totalSweeps)
            {
                if (LAttack1.complete)
                {
                    currentSweeps++;
                    LAttack1.Reset();
                    leftArm.SetBool("Attack1", true);

                    LAttack1.enabled = false;

                    RAttack1.enabled = true;
                    RAttack1.StartTimer();
                }

                if (RAttack1.complete)
                {
                    currentSweeps++;
                    rightArm.SetBool("Attack1", true);
                    RAttack1.Reset();
                    RAttack1.enabled = false;

                    LAttack1.enabled = true;
                    LAttack1.StartTimer();
                }
            }
            else if (currentSlams <= totalSlams)
            {
                if (currentSlams == 0 && !LAttack2.active && !LAttack2.complete)
                {
                    LAttack2.StartTimer();
                }
                if (LAttack2.complete)
                {
                    currentSlams++;
                    LAttack2.Reset();
                    leftArm.SetBool("Attack2", true);

                    LAttack2.enabled = false;

                    RAttack2.enabled = true;
                    RAttack2.StartTimer();
                }

                if (RAttack2.complete)
                {
                    currentSlams++;
                    rightArm.SetBool("Attack2", true);
                    RAttack2.Reset();
                    RAttack2.enabled = false;

                    LAttack2.enabled = true;
                    LAttack2.StartTimer();
                }
            }
            else
            {
                currentSweeps = 0;
                currentSlams = 0;
            }
        }
        else
        {
            Detach(gameObject);
        }

    }

    void Detach(GameObject target)
    {
        foreach (SpriteRenderer sprite in bossParts)
        {
            if (!sprite.GetComponent<Rigidbody2D>())
            {
                sprite.gameObject.AddComponent<Rigidbody2D>();
            }

            sprite.GetComponent<Rigidbody2D>().isKinematic = false;

            if (sprite.GetComponent<Animator>())
            {
                sprite.GetComponent<Animator>().enabled = false;
            }
            sprite.GetComponent<PolygonCollider2D>().enabled = true;

            if(sprite.GetComponent<Attack>())
            {
                sprite.GetComponent<Attack>().enabled = false;
            }
            sprite.transform.DetachChildren();

        }

        transform.DetachChildren();  
    }
}

