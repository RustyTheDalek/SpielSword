using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the right arm
/// </summary>
public class RightArm : MonoBehaviour {

	public Animator rArmRock1, rArmRock2, rArmRock3, rightArm,
	leftCrystal;

	public GameObject rockPileLeft;

	bool rockLeftRun;
	// Use this for initialization
	void Start () {
	
	}

	void SupportAttacks ()
	{
		if(rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamLeftRock") &&
			rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			rArmRock1.SetBool("Fall", true);
			rArmRock2.SetBool("Fall", true);
			rArmRock3.SetBool("Fall", true);
		}

		if(rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamLeftRock") &&
			rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f &&
			rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f &&
			!rockLeftRun)
		{
			//parent and move
			rockPileLeft.GetComponent<SpriteRenderer>().enabled = false;
			rockPileLeft.transform.parent = this.transform;
			rockPileLeft.transform.position = this.transform.localPosition;
			rockPileLeft.transform.rotation = this.transform.localRotation;
			rockPileLeft.transform.localPosition = new Vector3 (0,-2,0);
			rockPileLeft.GetComponent<SpriteRenderer>().enabled = true;
			rockLeftRun = true;
		}
		else if (rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamLeftRock") &&
				rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f )
		{
			rockPileLeft.transform.parent = null;
			rockPileLeft.transform.position = new Vector3 (8.5f, -7.5f, 0f);
			rockPileLeft.transform.rotation = new Quaternion (0, 0, 0, 0);
			rockLeftRun = false;
			//reset
		}

		if(rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamCrystal") ||
			rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSpecial"))
		{
			if(rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f &&
				rightArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.41f)
			{
				leftCrystal.SetBool("Rise", true);
			}
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		SupportAttacks ();
	}
}
