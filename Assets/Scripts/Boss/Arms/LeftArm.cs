using UnityEngine;
using System.Collections;
/// <summary>
/// Sync support attacks for the left arm
/// </summary>
public class LeftArm : MonoBehaviour {

	public GameObject rockPileRight;

	public Animator lArmRock1, lArmRock2, lArmRock3, lArmRock4, leftArm, rightCrystal;

	// Use this for initialization
	void Start () {}

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
		rockPileRight.transform.position = new Vector3 (-8.5f, -7.5f, 0f);
		rockPileRight.transform.rotation = new Quaternion (0, 0, 0, 0);
	}
}
