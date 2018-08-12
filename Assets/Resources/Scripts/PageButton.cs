using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageButton : MonoBehaviour {

    public GameObject page1;
    public GameObject page2;

    public Text text;

    private int page = 1;

    void Start()
    {

    }

    public void NextPage()
    {
        Debug.Log("Beep");

        if (page == 1)
        {
            Debug.Log("Bap");

            page1.SetActive(false);
            page2.SetActive(true);

            text.text = "Back";

            page = 2;
        }
        else if (page == 2)
        {
            SceneManager.LoadScene(0);
        }
    }
}
