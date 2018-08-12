using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour {

    public SpriteRenderer sprite;
    public TextMesh text;
    public GameManager gameManager;
    public GameAI gameAI;

    public string owner = "Blue";

    public List<GameObject> borderingTerritories = new List<GameObject>();

    public bool isBorderingPlayer;      // Checks if bordering a player's territory
    public bool underAttack = false;    // Checks if the territory is under attack   
    private bool playerControlled;      // Checks if the territory is controlled by the player
    [SerializeField]
    public bool isCastle;               // Checks if the territory has a castle which gives an additional defenders advantage
    [SerializeField]
    private bool isArmoury;             // Checks if the territory has an armoury which decreases the time it takes to create a new unit

    public int population = 0;          // The amount of defending units in the territory
    public int prepPopulation = 0;      // The ammount of attacking units in the territory

    private float clickDelay = 7f;      // The delay between clicks for adding/removing units when holding down the mouse button
    private float clickTime;            // ^^^ Used to keep track of this delay
    private float attackTime;           // The time it takes to kill one unit for the attacking side
    private float defendTime;           // The time it takes to kill one unit for the defending side
    private float defenseBonus = 0.25f; // The bonus kill time reduction given to defenders. Increases to 0.5f if the territory has a castle
    private float createTime;           // The amount of time it takes to create a unit
    private float createDelay = 4f;     // ^^^ Either 2f or 1.25f depending on whether or not the territory has an armoury
    
    // Use this for initialization
    void Start () {

        // General stuff
        text = gameObject.GetComponentInChildren<TextMesh>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameAI = GameObject.Find("GameAI").GetComponent<GameAI>();

        text.text = population.ToString();

        if (isCastle)
            defenseBonus = 0.5f;

        if (isArmoury)
            createDelay = 2.5f;

        ChangeColor();
        CheckBorders();
	}
	
	// Update is called once per frame
	void Update () {

        // Remove from unit creation timer if not under attack
        if (!underAttack)
            createTime -= Time.deltaTime;

        // General stuff
        if (population < 0)
            population = 0;

        if (prepPopulation < 0)
            prepPopulation = 0;

        if (prepPopulation <= 0)
        {
            text.text = population.ToString();
        }

        // Add to territory's population once creation timer reaches 0
        if (createTime <= 0 && !underAttack)
        {
            if (playerControlled)
                gameManager.units++;
            else
                gameAI.units++;

            createTime = createDelay;
        }

        #region Battle code

        // Battle code
        if (underAttack)
        {
            // Timer stuff
            attackTime -= Time.deltaTime;
            defendTime -= Time.deltaTime;

            // Kill defending unit if attack timer runs out
            if (attackTime <= 0)
            {
                population--;

                text.text = prepPopulation.ToString() + " vs " + population.ToString();

                attackTime = 3f - (Mathf.Clamp(prepPopulation / 5f, 1f, 2f));
            }

            // Kill attacking unit if defend timer runs out
            if (defendTime <= 0)
            {
                prepPopulation--;

                text.text = prepPopulation.ToString() + " vs " + population.ToString();

                defendTime = 3f - (Mathf.Clamp(population / 5f, 1f, 2f) + defenseBonus);
            }

            // Award attackers territory if they win
            if (population <= 0)
            {
                if (playerControlled)
                {
                    owner = "Red";

                    population = prepPopulation;
                    prepPopulation = 0;

                    underAttack = false;
                }
                else
                {
                    owner = "Blue";

                    population = prepPopulation;
                    prepPopulation = 0;

                    underAttack = false;
                }

                ChangeColor();
                gameAI.CheckForTerritories();
                gameManager.CheckRemainingTerritories();

                // Border update done to all territories for good measure
                foreach (Territory territory in FindObjectsOfType<Territory>())
                {
                    // isBorderingPlayer is set to false to initialize for the CheckBorders function
                    territory.isBorderingPlayer = false;
                    territory.CheckBorders();
                }
            }

            // Award defenders territory if they win
            if (prepPopulation <= 0)
            {
                if (playerControlled)
                {
                    owner = "Blue";

                    prepPopulation = 0;

                    underAttack = false;
                }
                else
                {
                    owner = "Red";

                    prepPopulation = 0;

                    underAttack = false;
                }

                ChangeColor();
                gameAI.CheckForTerritories();
                gameManager.CheckRemainingTerritories();

                // isBorderingPlayer is set to false to initialize for the CheckBorders function
                isBorderingPlayer = false;
                CheckBorders();
            }
        }
        #endregion
    }

    void OnMouseOver()
    {
        // Adjust functionality depending on if the territory is player or AI controlled
        if (playerControlled)
        {
            if (!underAttack)
            {
                // Add units
                if (Input.GetMouseButton(0))
                {
                    if (gameManager.units > 0 && Time.time >= clickTime)
                    {
                        population++;
                        gameManager.units--;

                        Debug.Log(gameObject.name + "'s population went up");

                        text.text = population.ToString();

                        clickTime = Time.time + 1 / clickDelay;
                    }
                }

                // Remove units
                if (Input.GetMouseButton(1) && population > 0 && Time.time >= clickTime)
                {
                    population--;
                    gameManager.units++;

                    Debug.Log(gameObject.name + "'s population went down");

                    text.text = population.ToString();

                    clickTime = Time.time + 1 / clickDelay;
                }
            }
        }
        else if (!playerControlled)
        {
            if (!underAttack)
            {
                // Add to prepping units
                if (Input.GetMouseButton(0) && gameManager.units > 0 && Time.time >= clickTime)
                {
                    if (isBorderingPlayer)
                    {
                        Debug.Log("beep");

                        prepPopulation++;
                        gameManager.units--;

                        text.text = prepPopulation.ToString() + " vs " + population.ToString();

                        clickTime = Time.time + 1 / clickDelay;
                    }
                }

                // Remove from prepping units
                if (Input.GetMouseButton(1) && prepPopulation > 0 && Time.time >= clickTime)
                {
                    if (isBorderingPlayer)
                    {
                        Debug.Log("beep");

                        prepPopulation--;
                        gameManager.units++;

                        text.text = prepPopulation.ToString() + " vs " + population.ToString();

                        clickTime = Time.time + 1 / clickDelay;
                    }
                }

                // Initiate attack
                if (Input.GetMouseButtonDown(2) && prepPopulation > 0)
                {
                    Debug.Log("Initiating attack in " + gameObject.name);

                    underAttack = true;

                    // Initialize battle
                    attackTime = 3f - (Mathf.Clamp(prepPopulation / 5f, 1f, 2f));
                    defendTime = 3f - (Mathf.Clamp(population / 5f, 1f, 2f) + defenseBonus);
                }
            }
        }
    }

    // Changes color to match the owner's
    public void ChangeColor()
    {

        if (owner == "Blue")
        {
            playerControlled = true;
        }
        else
        {
            playerControlled = false;
        }

    }

    // Checks to see if territory borders a player's territory
    void CheckBorders()
    {
        foreach (GameObject territory in borderingTerritories)
        {
            if (territory.GetComponent<Territory>().owner == "Blue" && !underAttack)
            {
                isBorderingPlayer = true;
            }
        }
    }
}
