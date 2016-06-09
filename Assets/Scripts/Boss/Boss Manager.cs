using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BossManager : MonoBehaviour {


	public static float health = 100;

	int[] stageOneAttacks, stageTwoAttacks, stageThreeAttacks, stageFourAttacks, stageFiveAttacks;
	
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

	List<float> healthRecord;

	public List<SpriteRenderer> bossParts;

	public List<Sprite> headStages, chestStages, lArmStages, rArmStages, c2, c3, c4;

	// Update is called once per frame
	public void ManagerUpdate ()
	{

		#region Debug options for testing
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			health = 100;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			health = 70;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			health = 50;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			health = 30;
		}
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			health = 10;
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			health = 0;
		}
		#endregion 

		#region Stadge select (case?)

		if (currentHealth > 80)
		{
			StadgeOne ();
		}
		else if(currentHealth < 80 && currentHealth > 60)
		{
			StadgeTwo ();
		}
		else if(currentHealth < 60 && currentHealth > 40)
		{
			StadgeThree ();
		}
		else if(currentHealth < 40 && currentHealth > 20)
		{
			StadgeFour ();
		}
		else if(currentHealth <= 0)
		{
			StadgeFive ();
		}
		#endregion

		currentHealth = health;

		if (!alive)
		{
			Detach(gameObject);
	    }
	}

	public abstract void StadgeOne ();
	public abstract void StadgeTwo ();
	public abstract void StadgeThree ();
	public abstract void StadgeFour ();
	public abstract void StadgeFive ();

	public abstract void SetBossParts ();

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
