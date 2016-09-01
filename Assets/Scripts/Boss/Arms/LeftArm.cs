using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the left arm
/// </summary>
public class LeftArm : MonoBehaviour {

	public GameObject rockPileRight;

	public Animator lArmRock1, lArmRock2, lArmRock3, lArmRock4, leftArm, rightCrystal;

	bool rockRightRun;
	// Use this for initialization
	void Start () {
	
	}
		
	void SupportAttacks ()
	{
		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		{
			lArmRock1.SetBool("Fall", true);
			lArmRock2.SetBool("Fall", true);
			lArmRock3.SetBool("Fall", true);
		}

		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftCenterSlamWithRocks") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			lArmRock1.SetBool("Center", true);
			lArmRock2.SetBool("Center", true);
			lArmRock3.SetBool("Center", true);
			lArmRock4.SetBool("Center", true);
		}

		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f &&
			!rockRightRun || 
			leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftCenterSlamWithRocks") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f &&
			!rockRightRun)
		{
			//parent and move
			rockPileRight.GetComponent<SpriteRenderer>().enabled = false;
			rockPileRight.transform.parent = this.transform;
			rockPileRight.transform.position = this.transform.position;
			rockPileRight.transform.rotation = this.transform.rotation;
			rockPileRight.transform.localPosition = new Vector3 (0,-2,0);
			rockPileRight.GetComponent<SpriteRenderer>().enabled = true;
			rockRightRun = true;
		}
		else if (leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
				leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f ||
				leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftCenterSlamWithRocks") &&
				leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
		{
			rockPileRight.transform.parent = null;
			rockPileRight.transform.position = new Vector3 (-8.5f, -7.5f, 0f);
			rockPileRight.transform.rotation = new Quaternion (0, 0, 0, 0);
			rockRightRun = false;
			//reset
		}

			
		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamCrystal") ||
			leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSpecial"))
		{
			if(leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f &&
				leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.41f)
			{
				rightCrystal.SetBool("Rise", true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		SupportAttacks ();
	}
}
