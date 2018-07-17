using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour {

    private Vector2 findPlayer;
    private int amountOfCharacters;
    private int selectedCharacters;
    private List<int> characters = new List<int>();
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
        // Lower case due to inputString
        validCharacters.Add("a", 0);
        validCharacters.Add("d", 1);
        validCharacters.Add("w", 2);
        validCharacters.Add("s", 3);
        validCharacters.Add("q", 4);
        validCharacters.Add("f", 5);
        validCharacters.Add("v", 6);
        validCharacters.Add("e", 7);
        validCharacters.Add("r", 8);
        validCharacters.Add("z",9);
        validCharacters.Add("x", 10);
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
        actPlayer.GetComponent<Villager>().moveDir =
                new Vector2(0, 0);

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
        int intOut;
        if(validCharacters.TryGetValue(playerInput, out intOut)){ }

        if (characters[progression] == intOut)
        {
            progression++;
        }

        if (progression == characters.Count)
        {
            actPlayer.GetComponent<Villager>().canAct = true;
            characters.Clear();
            restricted = false;
            animi.SetBool("Fire", false);
        }
    }
}
