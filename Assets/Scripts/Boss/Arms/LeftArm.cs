using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the left arm
/// </summary>
public class LeftArm : MonoBehaviour {

	public GameObject rockPileRight;

	public Animator lArmRock1, lArmRock2, lArmRock3,
	leftArm, rightCrystal;

	// Use this for initialization
	void Start () {
	
	}
		
	void SupportAttacks ()
	{
		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			lArmRock1.SetBool("Fall", true);
			lArmRock2.SetBool("Fall", true);
			lArmRock3.SetBool("Fall", true);
		}

		if(leftArm.GetCurrentAnimatorStateInfo(0).IsName("LeftSlamRightRock") &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f &&
			leftArm.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
		{
			//parent and move
			rockPileRight.transform.position = this.transform.position;
			rockPileRight.transform.rotation = this.transform.rotation;
			rockPileRight.transform.localPosition = new Vector3 (0,-2,0);
			rockPileRight.transform.parent = this.transform;
		}
		else
		{
			rockPileRight.transform.parent = null;
			rockPileRight.transform.position = new Vector3 (-8.5f, -7.5f, 0f);
			rockPileRight.transform.rotation = new Quaternion (0, 0, 0, 0);
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
