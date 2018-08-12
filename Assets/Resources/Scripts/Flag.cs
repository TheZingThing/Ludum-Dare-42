using UnityEngine;

public class Flag : MonoBehaviour {

    public Territory territory;     // The territory the flag is found in
    public SpriteRenderer sprite;   // Flag's SpriteRenderer component

    public Sprite blueFlag;         // Image of blue flag
    public Sprite redFlag;          // Image of red flag

	// Use this for initialization
	void Start () {

        territory = gameObject.GetComponentInParent<Territory>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
        switch (territory.owner)
        {
            case "Blue":
                {
                    sprite.sprite = blueFlag;

                    break;
                }
            case "Red":
                {
                    sprite.sprite = redFlag;

                    break;
                }

        }
	}
}
