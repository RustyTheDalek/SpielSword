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
    Dictionary<int, Sprite> spriteCharacters = new Dictionary<int, Sprite>();

    #region May need a sprite sheet or summit to handle this
    public Sprite letterA;
    public Sprite letterD;
    public Sprite letterW;
    public Sprite letterS;
    public Sprite letterQ;
    public Sprite letterF;
    public Sprite letterV;
    public Sprite letterE;
    public Sprite letterR;
    public Sprite letterZ;
    public Sprite letterX;
    #endregion

    // Use this for initialization
    void Start () {
        ValidCharacters();
        SpriteCharacters();
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
            Display();
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
        validCharacters.Add("z", 9);
        validCharacters.Add("x", 10);
    }

    void SpriteCharacters()
    {
        //note change to sprite sheet or atlas
        // Sets a sprite based on assigned number
        spriteCharacters.Add(0, letterA);
        spriteCharacters.Add(1, letterD);
        spriteCharacters.Add(2, letterW);
        spriteCharacters.Add(3, letterS);
        spriteCharacters.Add(4, letterQ);
        spriteCharacters.Add(5, letterF);
        spriteCharacters.Add(6, letterV);
        spriteCharacters.Add(7, letterE);
        spriteCharacters.Add(8, letterR);
        spriteCharacters.Add(9, letterZ);
        spriteCharacters.Add(10, letterX);

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
                amountOfCharacters = Random.Range(2, 3);
                break;
            case 1:
                amountOfCharacters = Random.Range(2, 5);
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
            Debug.Log("" + spriteCharacters[selectedCharacters]);

            progression = 0;
        }
        restricted = true;

    }

    void Display()
    {

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
            playerHere = false;
            restricted = false;
            animi.SetBool("Fire", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (LayerMask.NameToLayer("Villager")))
        {
            actPlayer = collision.gameObject;
            playerHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (LayerMask.NameToLayer("Villager")))
        {
            actPlayer = null;
            playerHere = false;
        }
    }
}
