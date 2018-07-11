using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour {

    private Vector2 findPlayer;
    private int amountOfCharacters;
    private int selectedCharacters;
    private List<int> characters = new List<int>();
    //private int[] characters = new int[10];
    private int progression;
    private bool restricted;

    public Animator animi;
    public int difficulty;
    public GameObject actPlayer;
    public LayerMask layerGroundOnly;
    public bool playerHere;
    public string playerInput;
    Dictionary<string, int> validCharacters = new Dictionary<string, int>();

    // Use this for initialization
    void Start () {
        ValidCharacters();
        restricted = false;
        playerHere = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        // Get a player check from PlayerCheck to see if player is present
        if (playerHere&& !restricted)
        {
            FindFoe();
        }
        if (restricted)
        {
            if (Input.anyKeyDown)
            {
                playerInput = Input.inputString;
            }
            restrict();
        }
        
    }

    void ValidCharacters()
    {
        validCharacters.Add("A", 0);
        validCharacters.Add("D", 1);
        validCharacters.Add("W", 2);
        validCharacters.Add("S", 3);
        validCharacters.Add("Q", 4);
        validCharacters.Add("F", 5);
        validCharacters.Add("C", 6);
        validCharacters.Add("E", 7);
        validCharacters.Add("R", 8);
        validCharacters.Add("Z",9);
        validCharacters.Add("X", 10);
    }

    void FindFoe()
    {
        findPlayer = new Vector2(actPlayer.transform.position.x - transform.position.x, actPlayer.transform.position.y - transform.position.y).normalized;
        bool rayResult = Physics2D.Raycast(transform.position, findPlayer, 3.5f, layerGroundOnly);
        if (rayResult)
        { 
            Attack();
        }
    }

    void Attack()
    {
        //disable player movment
        actPlayer.GetComponent<Villager>().canAct = false;

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
            characters.Add(selectedCharacters);
            Debug.Log("" + selectedCharacters);

            progression = 0;
        }
        restricted = true;

    }

   void restrict()
    {
        if (progression == characters.Count)
        {
            actPlayer.GetComponent<Villager>().canAct = true;
            characters.Clear();
            restricted = false;
            animi.SetBool("Fire", false);
        }

        if (characters[progression] == validCharacters[playerInput])
        {
            progression++;
        }


        //switch (playerInput)
        //{
        //    case "A":
        //        //if (characters[progression] == validCharacters.ContainsValue("A"))
        //        //thing
        //        break;
        //    case "D":
        //        //thing
        //        break;
        //    case "W":
        //        //thing
        //        break;
        //    case "S":
        //        //thing
        //        break;
        //    case "Q":
        //        //thing
        //        break;
        //    case "F":
        //        //thing
        //        break;
        //    case "C":
        //        //thing
        //        break;
        //    case "E":
        //        //thing
        //        break;
        //    case "R":
        //        //thing
        //        break;
        //    case "Z":
        //        //thing
        //        break;
        //    case "X":
        //        //thing
        //        break;
        //    default:
        //        Debug.Log("Non valid key: " + playerInput);
        //        break;
        //}

    }
}
