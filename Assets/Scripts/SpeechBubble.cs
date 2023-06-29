using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] float bubbleDuration = 2;
    [SerializeField] Sprite boxBubble;
    [SerializeField] Sprite buttonBubble;
    [SerializeField] Sprite nopeBubble;
    [SerializeField] Sprite spaceshipBubble;

    SpriteRenderer sprRen;
    bool isShowingBubble;

    void Start()
    {
        sprRen = GetComponent<SpriteRenderer>();
    }

    // Choose what to show based on nearby objects
    void OnTriggerEnter2D(Collider2D other) {

        GameObject obj = other.gameObject;

        // If the object is a spaceship...
        Spaceship spaceship = obj.GetComponent<Spaceship>();
        if (spaceship != null)
            // If it is closed, show a "button" bubble
            if (!spaceship.IsOpen())
                ShowBubble("button");
                
        // If the object is a button...
        Button button = obj.GetComponent<Button>();
        if (button != null)
            // If the button is pressed, show a "spaceship" bubble
            if (button.IsPressed())
                ShowBubble("spaceship");
            // If the button is not pressed, show a "box" bubble
            else
                ShowBubble("box");
    }

    public void ShowBubble(string bubbleType)
    {
        // If a bubble is already showing, ignore.
        if (isShowingBubble == true) return;
        
        // Get sprite
        Sprite sprite = null;

        switch (bubbleType)
        {
            case "box":
                sprite = boxBubble;
                break;
            case "button":
                sprite = buttonBubble;
                break;
            case "nope":
                sprite = nopeBubble;
                break;
            case "spaceship":
                sprite = spaceshipBubble;
                break;
        }
        
        // Show sprite for duration
        StartCoroutine(ShowBubbleWithDuration(sprite));
    }

    IEnumerator ShowBubbleWithDuration(Sprite sprite)
    {
        isShowingBubble = true;

        sprRen.sprite = sprite;
        yield return new WaitForSeconds(bubbleDuration);
        sprRen.sprite = null;

        isShowingBubble = false;
    }
}
