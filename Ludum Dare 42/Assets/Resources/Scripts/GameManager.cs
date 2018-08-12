using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int units = 0;

    public Text text;

    private int blueTer;
    private int redTer; 

    void Start()
    {
        CheckRemainingTerritories();
    }

    void Update()
    {
        text.text = "Units: " + units.ToString();           
    }

    public void CheckRemainingTerritories()
    {
        blueTer = 0;
        redTer = 0;

        foreach (Territory territory in FindObjectsOfType<Territory>())
        {
            if (territory.owner == "Blue")
                blueTer++;

            if (territory.owner == "Red")
                redTer++;
        }

        if (blueTer == 0)
        {
            SceneManager.LoadScene(4);
        }

        if (redTer == 0)
        {
            SceneManager.LoadScene(3);
        }
    }

}
