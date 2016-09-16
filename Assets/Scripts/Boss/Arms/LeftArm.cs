using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the left arm
/// </summary>
public class LeftArm : MonoBehaviour {

	public GameObject rockPileRight;

	public Animator lArmRock1, lArmRock2, lArmRock3, lArmRock4, leftArm, rightCrystal;

	bool flyOff;
	// Use this for initialization
	void Start () 
	{
		flyOff = false;
	}

	/// <summary>
	/// Causes the Left crystal to rise.
	/// </summary>
	void LeftCrystal ()
	{
		rightCrystal.SetBool("Rise", true);
	}
	/// <summary>
	/// Causes the rocks for the left arm to fall on the left side of the screen
	/// </summary>
	void LeftSlamRightRock()
	{
		lArmRock1.SetBool("Fall", true);
		lArmRock2.SetBool("Fall", true);
		lArmRock3.SetBool("Fall", true);
	}
	/// <summary>
	/// Cause the center rocks to fall down
	/// </summary>
	void LeftCenterSlamWithRocks ()
	{
		lArmRock1.SetBool("Center", true);
		lArmRock2.SetBool("Center", true);
		lArmRock3.SetBool("Center", true);
		lArmRock4.SetBool("Center", true);
	}
	/// <summary>
	/// Attach the boulder to the arm
	/// </summary>
	void LeftBoulderAttach ()
	{
		rockPileRight.GetComponent<SpriteRenderer>().enabled = false;
		flyOff = false;
		rockPileRight.transform.parent = this.transform;
		rockPileRight.transform.position = this.transform.position;
		rockPileRight.transform.rotation = this.transform.rotation;
		rockPileRight.transform.localPosition = new Vector3 (0,-2,0);
		rockPileRight.GetComponent<SpriteRenderer>().enabled = true;
	}
	/// <summary>
	/// Detach the boulder from the arm
	/// </summary>
	void LeftBoulderDetach ()
	{
		rockPileRight.transform.parent = null;
		flyOff = true;
	}
	/// <summary>
	/// Update the position and rotation of the rocks once they detatch.
	/// </summary>
	void Update ()
	{
		if(flyOff)
		{
			if(rockPileRight.transform.position.y <= 13)
			{
				rockPileRight.transform.Rotate (0, 0, 640 * Time.deltaTime, Space.World);
				rockPileRight.transform.Translate(-10 * Time.deltaTime, 60 * Time.deltaTime, 0, Space.World);
			}
		}
	}
}
