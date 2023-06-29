using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{

    [SerializeField] UnityEvent onPress;
    [SerializeField] UnityEvent onRelease;

    Animator animator;
    BoxCollider2D boxCollider;

    List<GameObject> collidedObjects;

    // Start is called before the first frame update
    void Start()
    {
        collidedObjects = new List<GameObject>();

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool IsPressed()
    {
        return collidedObjects.Count > 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "CantActivateTriggers") return;

        collidedObjects.Add(other.gameObject);
        UpdateAnimatorParameter();

        // Invoke callbacks
        if (collidedObjects.Count == 1)
            onPress.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "CantActivateTriggers") return;
        
        collidedObjects.Remove(other.gameObject);
        UpdateAnimatorParameter();

        // Invoke callbacks
        if (collidedObjects.Count == 0)
            onRelease.Invoke();
    }

    private void UpdateAnimatorParameter() {
        if (collidedObjects.Count > 0)
            animator.SetBool("isPressed", true);
        else
            animator.SetBool("isPressed", false);
    }

}
