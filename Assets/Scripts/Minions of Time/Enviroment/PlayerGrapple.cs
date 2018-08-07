using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour {
    #region Private Variables
    private Vector2 findPlayer;
    private int amountOfCharacters;
    private int selectedCharacters;
    private int progression;
    private float timer;
    private bool playerHere;
    private bool restricted;
    private bool fired;
    private Vector3 playerLocation;
    private List<int> characters = new List<int>();
    private List<GameObject> trapSprites = new List<GameObject>();
    private SpriteRenderer fwd_SpriteRenderer;
    private SpriteRenderer bk_SpriteRenderer;
    #endregion

    #region Public Variables
    public Animator animi;
    public int difficulty;
    public GameObject actPlayer;
    public LayerMask layerGroundOnly;
    public string playerInput;
    public string playerInputPast;

    #region List of sprites
    //May need a sprite sheet or summit to handle this
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

    #region List of sprite objects
    // Note only need a preset amount of these
    // no need for each trap to have them
    public GameObject trapSprite1;
    public GameObject trapSprite2;
    public GameObject trapSprite3;
    public GameObject trapSprite4;
    public GameObject trapSprite5;
    #endregion

    #region Object to change colour
    public GameObject fwdSprite;
    public GameObject bkSprite;
    #endregion

    #endregion

    #region Dictionary
    Dictionary<string, int> validCharacters = new Dictionary<string, int>();
    Dictionary<int, Sprite> spriteCharacters = new Dictionary<int, Sprite>();
    #endregion
    
    
    void Start () {
        ValidCharacters();
        SpriteCharacters();
        TrapSpritesList();
        fwd_SpriteRenderer = fwdSprite.GetComponent<SpriteRenderer>();
        bk_SpriteRenderer = bkSprite.GetComponent<SpriteRenderer>();
        restricted = false;
        playerHere = false;
    }
	
	void Update () {

        if (timer >= 3.0f)
        {
            fired = false;
            fwd_SpriteRenderer.color = Color.white;
            bk_SpriteRenderer.color = Color.white;
        }

        if (fired)
        {
            timer += Time.deltaTime;
        }

        // Get a player check from PlayerCheck to see if player is present
        if (playerHere && !restricted && !fired)
        {
            playerLocation = actPlayer.transform.position;
            FindFoe();
            Display(0);
        }
        if (restricted)
        {
            
            if (Input.anyKeyDown)
            {
                playerInput = Input.inputString;
            }
            Restrict();
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

    void TrapSpritesList()
    {
        //adds the game objects to a list to set later
        trapSprites.Add(trapSprite1);
        trapSprites.Add(trapSprite2);
        trapSprites.Add(trapSprite3);
        trapSprites.Add(trapSprite4);
        trapSprites.Add(trapSprite5);
    }

    void FindFoe()
    {
        findPlayer = new Vector2(playerLocation.x - transform.position.x,
            playerLocation.y - transform.position.y).normalized;
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

        //sets the amount of letters to string togetter
        switch (difficulty)
        {
            case 0:
                amountOfCharacters = Random.Range(1, 3);
                break;
            case 1:
                amountOfCharacters = Random.Range(2, 5);
                break;
        }
        //sets a character for each member of the string
        for (int i = 0; i <= amountOfCharacters; i++)
        {
            //sets the range of letters to pick from
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
            //Debug.Log("" + spriteCharacters[selectedCharacters]);

            progression = 0;
        }
        restricted = true;

    }

    void Display(int progression)
    {
        for (int i = progression; i <= characters.Count - 1; i++)
        {
            // Sets the first object to the first letter and so on
            trapSprites[i - progression].GetComponent<SpriteRenderer>().sprite =
                spriteCharacters[characters[i]];

            // Position off set by current object count
            trapSprites[i - progression].transform.position =
                new Vector2(playerLocation.x + 1 + (i * 0.65f),
                playerLocation.y + 1.7f);

        }
    }

    void Restrict()
    {
        int intOut;
        if (validCharacters.TryGetValue(playerInput, out intOut)) {}
        else if (playerInput != playerInputPast)
        {
            progression = 0;
            Display(progression);
        }

        if (characters[progression] == intOut &&
            playerInput != playerInputPast)
        {
            progression++;
            Display(progression);
        }
        else if (characters[progression] != intOut &&
            playerInput != playerInputPast)
        {
            progression = 0;
            Display(progression);
        }

        playerInput = playerInputPast;

        if (progression == characters.Count)
        {
            Reset();
        }
    }

    void Reset()
    {
        actPlayer.GetComponent<Villager>().canAct = true;
        characters.Clear();
        playerHere = false;
        restricted = false;
        fired = true;
        fwd_SpriteRenderer.color = Color.gray;
        bk_SpriteRenderer.color = Color.gray;
        timer = 0;
        animi.SetBool("Fire", false);
        for (int i = 0; i <= 4; i++)
        {
            // Resets the sprite
            trapSprites[i].GetComponent<SpriteRenderer>().sprite = null;
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
