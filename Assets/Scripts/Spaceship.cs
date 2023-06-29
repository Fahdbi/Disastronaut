using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spaceship : MonoBehaviour
{  
    [SerializeField] UnityEvent onLevelComplete;

    Animator anim;
    bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetBool("isOpen", true);
        isOpen = true;
    }

    public void Close()
    {
        anim.SetBool("isOpen", false);
        isOpen = false;
    }
    
    public bool IsOpen()
    {
        return isOpen;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "CantActivateTriggers") return;

        // If the spaceship is closed
        if (!isOpen) 
            return;

        // Check if the other object is a Player
        GameObject otherObject = other.gameObject; 
        if (otherObject.GetComponent<Player>() != null) 
        {
            Debug.Log("Level complete!");
            onLevelComplete.Invoke();
        }
    }

}
