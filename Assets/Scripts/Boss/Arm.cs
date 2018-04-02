using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Sync support attacks for the arms
/// (Created from the RightArm & LeftArm scripts as they did the same thing)
/// Created by : Ian Jones      - 02/04/18
/// Updated by : Ian Jones      - 02/04/18
/// </summary>
public class Arm : MonoBehaviour {

	public GameObject rockPile;

	public Animator arm, crystal;

    public List<Animator> rocks;

	bool flyOff;

	// Use this for initialization
	void Start () 
	{
		flyOff = false;
	}

	/// <summary>
	/// Causes the crystal to rise.
	/// </summary>
	void Crystal ()
	{
        crystal.SetBool("Rise", true);
	}
	/// <summary>
	/// Causes the rocks for the arm to fall
	/// </summary>
	void SlamRock()
	{
        foreach(Animator rock in rocks)
        {
            rock.SetBool("Fall", true);
        }
	}

	/// <summary>
	/// Cause the center rocks to fall down
	/// </summary>
	void CenterSlamWithRocks ()
	{
        foreach(Animator rock in rocks)
        {
            rock.SetBool("Center", true);
        }
	}

	/// <summary>
	/// Attach the boulder to the arm
	/// </summary>
	void BoulderAttach ()
	{
        rockPile.GetComponent<SpriteRenderer>().enabled = false;
		flyOff = false;
        rockPile.transform.parent = this.transform;
        rockPile.transform.position = this.transform.position;
        rockPile.transform.rotation = this.transform.rotation;
        rockPile.transform.localPosition = new Vector3 (0,-2,0);
        rockPile.GetComponent<SpriteRenderer>().enabled = true;
	}

	/// <summary>
	/// Detach the boulder from the arm
	/// </summary>
	void BoulderDetach ()
	{
        rockPile.transform.parent = null;
		flyOff = true;
	}

	/// <summary>
	/// Update the position and rotation of the rocks once they detatch.
	/// </summary>
	void Update ()
	{
		if(flyOff)
		{
			if(rockPile.transform.position.y <= 13)
			{
                rockPile.transform.Rotate (0, 0, 640 * Time.deltaTime, Space.World);
                rockPile.transform.Translate(-10 * Time.deltaTime, 60 * Time.deltaTime, 0, Space.World);
			}
		}
	}
}
