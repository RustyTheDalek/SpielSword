using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the right arm
/// </summary>
public class RightArm : MonoBehaviour {

	public Animator rArmRock1, rArmRock2, rArmRock3, rightArm,
	leftCrystal;

	public GameObject rockPileLeft;

	bool flyOff;
	// Use this for initialization
	void Start () 
	{
		flyOff = false;
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
		flyOff = false;
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
		flyOff = true;
	}
	/// <summary>
	/// Update the position and rotation of the rocks once they detatch.
	/// </summary>
	void Update ()
	{
		if(flyOff)
		{
			if(rockPileLeft.transform.position.y <= 13)
			{
				rockPileLeft.transform.Rotate (0, 0, -640 * Time.deltaTime, Space.World);
				rockPileLeft.transform.Translate(10 * Time.deltaTime, 60 * Time.deltaTime, 0, Space.World);
			}
		}
	}
}
