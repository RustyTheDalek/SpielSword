using System.Collections;
using UnityEngine;

/// <summary>
/// Controller script for the World map villager
/// Created by : Ian Jones - 10/04/18
/// </summary>
[RequireComponent(typeof(MapCharacter2D))]
public class MapVillager : MonoBehaviour
{
    public Vector2 direction;

    MapCharacter2D m_MapCharacter;

    Hashtable animData;

    private void Awake()
    {
        m_MapCharacter = GetComponent<MapCharacter2D>();

        animData = new Hashtable
        {
            { "Direction", Vector2.zero }
        };

    }

	// Update is called once per frame
	void Update ()
    {
        direction = Vector2.zero;
        direction = ((Input.GetKey(KeyCode.D)) ? Vector2.right : direction);
        direction = ((Input.GetKey(KeyCode.A)) ? Vector2.left: direction);
        direction = ((Input.GetKey(KeyCode.W)) ? Vector2.up : direction);
        direction = ((Input.GetKey(KeyCode.S)) ? Vector2.down : direction);
    }

    private void FixedUpdate()
    {
        animData["Direction"] = direction;
        m_MapCharacter.Move(animData);
    }
}
