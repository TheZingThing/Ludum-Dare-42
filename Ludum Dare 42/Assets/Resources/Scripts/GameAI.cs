using System.Collections.Generic;
using UnityEngine;

public class GameAI : MonoBehaviour {

    List<Territory> territories = new List<Territory>();    // List of all the territories the AI controls

    public string playerColor = "Red";                      // The Color which the AI controls
    public int units = 0;                                   // The amount of units the AI has

    private float attackDelay = 5f;                         // Time before the AI can attack
    private float timer = 120f;                              // A timer that dictates the AI's decisions

	// Use this for initialization
	void Start () {

        CheckForTerritories();

	}
	
	// Update is called once per frame
	void Update () {
		
        foreach (Territory territory in territories)
        {

            if (!territory.isBorderingPlayer && !territory.underAttack)
            {
                if (territory.population > 0)
                {
                    units += territory.population;

                    territory.population = 0;

                    territory.text.text = territory.population.ToString();
                }
            }

            // Check if AI needs to reinforce territory
            if (territory.isBorderingPlayer && !territory.underAttack)
            {
                if (timer >= 90f)
                {
                    if (territory.population < 8 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
                else if (timer >= 70f)
                {
                    if (territory.population < 14 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
                else if (timer >= 50f)
                {
                    if (territory.population < 20 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
                else if (timer >= 30f)
                {
                    if (territory.population < 25 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
                else if (timer >= 10f)
                {
                    if (territory.population < 30 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
                else if (timer <= 0f)
                {
                    if (territory.population < 35 && units > 0)
                    {
                        territory.population++;

                        units--;

                        if (territory.prepPopulation > 1)
                            territory.text.text = territory.prepPopulation.ToString() + " vs " + territory.population.ToString();
                        else
                            territory.text.text = territory.population.ToString();
                    }
                }
            }

            if (territory.isBorderingPlayer && attackDelay <= 0)
            {
                foreach (GameObject border in territory.borderingTerritories)
                {
                    if (border.GetComponent<Territory>().owner == "Blue" && !territory.underAttack)
                    {
                        // border is the used to get the game object in the borderingTerritories list, ter is a variable which contains it's Territory component (i.e ter is used for simplification)
                        var ter = border.GetComponent<Territory>();

                        // Check if AI can attack enemy territory
                        if (!ter.underAttack)
                        {
                            // Change amount of units sent depending on whether or not the territory has a castle
                            if (!ter.isCastle)
                            {
                                if (units > ter.population + 4)
                                {
                                    ter.prepPopulation = ter.population + 4;
                                    units -= ter.population + 4;

                                    ter.text.text = ter.prepPopulation.ToString() + " vs " + ter.population.ToString();

                                    ter.underAttack = true;
                                }
                            }
                            else
                            {
                                if (units > ter.population + 8)
                                {
                                    ter.prepPopulation = ter.population + 8;
                                    units -= ter.population + 8;

                                    ter.text.text = ter.prepPopulation.ToString() + " vs " + ter.population.ToString();

                                    ter.underAttack = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (attackDelay > 0)
            attackDelay -= Time.deltaTime;

        if (timer > 0)
            timer -= Time.deltaTime;
	}

    public void CheckForTerritories()
    {
        territories.Clear();

        foreach (Territory territory in FindObjectsOfType<Territory>())
        {
            if (territory.owner == playerColor)
            {
                territories.Add(territory);
            }
        }
    }
}
