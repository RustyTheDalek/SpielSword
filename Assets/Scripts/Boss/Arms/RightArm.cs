using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the right arm
/// </summary>
public class RightArm : MonoBehaviour {

	public Animator rArmRock1, rArmRock2, rArmRock3, rightArm,
	leftCrystal;

	public GameObject rockPileLeft;

	// Use this for initialization
	void Start () {
	
	}

	void SupportAttacks ()
	{
		//stored code for future use
		//rightArm.GetCurrentAnimatorStateInfo(0).IsName("RightSlamLeftRock") 
	}

	/// <summary>
	/// Causes the Right crystal to rise.
	/// </summary>
	void RightCrystal ()
	{
		leftCrystal.SetBool("Rise", true);
	}
	/// <summary>
	/// Causes the rocks for the right arm to fall on the right side of the screen
	/// </summary>
	void RightSlamLeftRock()
	{
		rArmRock1.SetBool("Fall", true);
		rArmRock2.SetBool("Fall", true);
		rArmRock3.SetBool("Fall", true);
	}
	/// <summary>
	/// Attach the boulder to the arm
	/// </summary>
	void RightBoulderAttach ()
	{
		rockPileLeft.GetComponent<SpriteRenderer>().enabled = false;
		rockPileLeft.transform.parent = this.transform;
		rockPileLeft.transform.position = this.transform.localPosition;
		rockPileLeft.transform.rotation = this.transform.localRotation;
		rockPileLeft.transform.localPosition = new Vector3 (0,-2,0);
		rockPileLeft.GetComponent<SpriteRenderer>().enabled = true;
	}
	/// <summary>
	/// Detach the boulder from the arm
	/// </summary>
	void RightBoulderDetach ()
	{
		rockPileLeft.transform.parent = null;
		rockPileLeft.transform.position = new Vector3 (8.5f, -7.5f, 0f);
		rockPileLeft.transform.rotation = new Quaternion (0, 0, 0, 0);
	}
}
