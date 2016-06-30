using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BossManager : MonoBehaviour {

	public GameObject attackManager;

	public static float health = 100;

	public List<int> attackList = new List<int>();

	public int attackCountStage;

	public int startTimer = 4;
	
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

	public List<Sprite> headStages, bodyStages, outterLArmStages, lArmStages, outterRArmStages, 
	rArmStages, lFootStages, rFootStages, tailStages, utilityA, utilityB, utilityC;

	// Update is called once per frame
	public virtual void Update ()
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
		if(startTimer == 4){
			if (currentHealth > 80)
			{
				StageOne ();
			}
			else if(currentHealth < 80 && currentHealth > 60)
			{
				StageTwo ();
			}
			else if(currentHealth < 60 && currentHealth > 40)
			{
				StageThree ();
			}
			else if(currentHealth < 40 && currentHealth > 20)
			{
				StageFour ();
			}
			else if(currentHealth <= 0)
			{
				StageFive ();
			}
			#endregion

			currentHealth = health;
		}
		if (!alive)
		{
			Detach(gameObject);
	    }
	}

	public abstract void StageOne ();
	public abstract void StageTwo ();
	public abstract void StageThree ();
	public abstract void StageFour ();
	public abstract void StageFive ();

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
