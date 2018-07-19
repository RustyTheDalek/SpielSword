﻿using UnityEngine;

/// <summary>
/// Script for Rogue Class, handles the teleporting of the player
/// Created by : Ian Jones - 04/11/17
/// Updated by : Ian Jones - 07/04/18
/// </summary>
public class Rogue : Villager {

    /// <summary>
    /// How far the Rogue can Teleport
    /// </summary>
    public float distance = 5;

    /// <summary>
    /// Collision information for Teleport
    /// </summary>
    public RaycastHit2D teleportTest;

    /// <summary>
    /// How much to offset the height for circle cast as the Pivot for Villagers is at their f
    /// </summary>
    float yOffset = .5f;

    /// <summary>
    /// What layers count as collision for teleport
    /// </summary>
    public LayerMask collisionLayers;

    //TODO : Look into this, little janky at the moment
    //TODO this current ability is more like a dash than a teleport since it can't go through things
    public void Blink()
    {
        //Figure out which was Rogue is facing
        float direction = melee.transform.position.x - transform.position.x;

        //Test in that direction to see if there is something in way
        teleportTest = Physics2D.CircleCast(transform.position + Vector3.up * yOffset, .5f, Vector3.right * Mathf.Sign(direction), distance, collisionLayers);

        Vector3 newPos;

        //If a collider was hit then we need to use the position of the circle cast as the new position
        if (teleportTest.collider)
        {
            newPos = (Vector3)teleportTest.centroid;
            Debug.Log("Didn't teleport full distance because: " + teleportTest.collider.name + " was in the way");
        }
        else //Nothing was hit within the Distance so we can teleport the full distance
        {
            newPos = transform.position + Vector3.right * Mathf.Sign(direction) * distance;
        }

        transform.position = newPos;
    }
}
