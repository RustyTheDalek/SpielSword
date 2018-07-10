using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour {

    private Vector2 findPlayer;
    private int amountOfCharacters;
    private int selectedCharacters;
    private int[] characters;

    public Animator animi;
    public int difficulty;
    public GameObject actPlayer;
    public LayerMask layerGroundOnly;
    public bool playerHere;
    Dictionary<int, string> validCharacters = new Dictionary<int, string>();

    // Use this for initialization
    void Start () {
        ValidCharacters();
    }
	
	// Update is called once per frame
	void Update () {
        
        // Get a player check from PlayerCheck to see if player is present
        if (playerHere)
        {
            FindFoe();
        }

    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x, actPlayer.transform.position.y - transform.position.y).normalized;
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (!rayResult)
        { 
            Attack();
        }
    }

    void Attack()
    {
        //disable player movment
        animi.SetBool("Fire", true);
        switch (difficulty)
        {
            case 0:
                amountOfCharacters = Random.Range(1, 3);
                break;
            case 1:
                amountOfCharacters = Random.Range(1, 5);
                break;
        }
        for (int i = 0; i <= amountOfCharacters; i++)
        {
            switch (difficulty)
            {
                case 0:
                    selectedCharacters = Random.Range(0, 3);
                    break;
                case 1:
                    selectedCharacters = Random.Range(0, 10);
                    break;
            }
            characters[i] = selectedCharacters;
        }

    }

    void ValidCharacters()
    {
        validCharacters.Add(0, "A");
        validCharacters.Add(1, "D");
        validCharacters.Add(2, "W");
        validCharacters.Add(3, "S");
        validCharacters.Add(4, "Q");
        validCharacters.Add(5, "F");
        validCharacters.Add(6, "C");
        validCharacters.Add(7, "E");
        validCharacters.Add(8, "R");
        validCharacters.Add(9, "Z");
        validCharacters.Add(10, "X");
    }
}
