using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Head : MonoBehaviour {

    public Golem boss;

    public Sprite originalHead;
    public Sprite damageHead;
	public Sprite hpBarVun;
	public Sprite hpBarInv;
	Image hpBar;

	public bool ifAttack;
	GameObject attackManager;

    Timer damageTimer;
	// Use this for initialization
	void Start () {
		//canAttack = true;
        damageTimer = gameObject.AddComponent<Timer>();
        damageTimer.Setup("Damager", .25f, true);
		hpBar = GameObject.Find ("boss health fill").GetComponent<Image>();
		attackManager = GameObject.Find("Attack Manager");
	}
	
	// Update is called once per frame
	void Update () {
		ifAttack = attackManager.GetComponent<AttackStorage>().canAttack;
        if(damageTimer.complete)
        {
            GetComponent<SpriteRenderer>().sprite = originalHead;
        }
		if(ifAttack)
		{
			hpBar.sprite = hpBarVun;
		}
		else if (!ifAttack)
		{
			hpBar.sprite = hpBarInv;
		}
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log("Collision");

		if(coll.transform.name == "Melee" && ifAttack)
        {
			Golem.health--;
			GetComponent<SpriteRenderer>().sprite = damageHead;
            damageTimer.StartTimer();
        }
    }
}
