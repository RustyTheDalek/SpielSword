using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls parralax movement of Background details for level
/// Created by : Ian Joens - 19/04/18
/// </summary>
public class BackgroundParrallax : MonoBehaviour {

    /// <summary>
    /// List of the level backgrounds
    /// </summary>
    public List<GameObject> backgrounds;

    /// <summary>
    /// Stores previous camera position for calculate delta position
    /// </summary>
    Vector2 prevCamPos = Vector2.zero;

    /// <summary>
    /// How much to slow Y movement of backgrounds by
    /// </summary>
    public float yDampen;

	void Start ()
    {
        UpdateParrallax();
    }
	
	void LateUpdate ()
    {
        UpdateParrallax();
	}

    void UpdateParrallax()
    {
        //Calculate difference in camera positions
        Vector3 deltaCamPos = (Vector2)Camera.main.transform.position - prevCamPos;

        //Loop through backgrounds and move their position
        foreach (GameObject background in backgrounds)
        {
            //Uses the Z depth of the background to dampen it's movement so the further 
            //back it is the slower it moves
            background.transform.position -= new Vector3(
                deltaCamPos.x / background.transform.position.z,
                deltaCamPos.y / (background.transform.position.z * yDampen), 0);
        }

        //For next frame
        prevCamPos = Camera.main.transform.position;
    }
}
