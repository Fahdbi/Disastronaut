using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float jumpSpeed = 3.0f;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float climbSpeed = 1.5f;
    [SerializeField] float reach = 2f;

    [Header("Colliders")]
    [SerializeField] Collider2D feetCollider;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask climbLayer;

    [Header("Offsets")]
    [SerializeField] Vector3 heldItemOffset = new Vector3(0, 1, 0);

    Rigidbody2D rb2d;
    SpriteRenderer sprRen;
    Animator anim;
    SpeechBubble speechBubble;
    TouchInput touchInput;

    Grabbable nearestGrabbable;
    Grabbable heldItem;
    bool gripObtained = false;
    bool isClimbing = false;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        speechBubble = GetComponentInChildren<SpeechBubble>();

        touchInput = FindObjectOfType<TouchInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Walk();

            // Don't jump or climb if the player is holding something
            CheckForClimb();
                
            if (!isClimbing)     // Climbing detection has precedence over jumping
                CheckForJump();

            // Find the nearest grabbable in preparation for grabbing 
            FindNearestGrabbable();
            
             // Update the item being held
            UpdateHeldItem();
            
            // Check whether the player pressed space
            CheckForGrabOrRelease();
        }

        Animate();

        UpdateGravityScale();
    }

    void Walk()
    {
        // Otherwise, give player velocity according to player input
        float xAxis = Input.GetAxis("Horizontal") + touchInput.GetAxis("Horizontal");
        xAxis = Mathf.Clamp(xAxis, -1, 1);

        rb2d.velocity = new Vector2(xAxis * moveSpeed, rb2d.velocity.y);
    }

    void FindNearestGrabbable()
    {
        // Ignore if player is holding something
        if (heldItem != null) 
            return;

        // Disable the outline of the last nearest grabbable, if it exists
        if (nearestGrabbable != null) 
            nearestGrabbable.GetComponent<LineRenderer>().enabled = false;
        
        // Find a nearby grabbable
        Vector2 playerPos = transform.position;

        Grabbable[] grabbables = FindObjectsOfType<Grabbable>();
        Grabbable nearestGrabbableEncountered = null;
        float nearestDistanceEncountered = reach;

        foreach (Grabbable grabbable in grabbables)
        {
            Vector2 grabbablePos = grabbable.transform.position;
            float grabbableDist = Vector2.Distance(playerPos, grabbablePos);

            if (grabbableDist < nearestDistanceEncountered) {
                nearestGrabbableEncountered = grabbable;
                nearestDistanceEncountered = grabbableDist;
            }
        }
        
        nearestGrabbable = nearestGrabbableEncountered;

        // Enable the outline of the grabbable
        if (nearestGrabbable != null)
            nearestGrabbable.GetComponent<LineRenderer>().enabled = true;
    }

    void CheckForClimb()
    {
        // Ignore if the player is holding something
        if (heldItem != null) 
        {
            isClimbing = false;
            return;
        }

        // Check if player is touching the ladder
        float yAxis = Input.GetAxis("Vertical") + touchInput.GetAxis("Vertical");
        yAxis = Mathf.Clamp(yAxis, -1, 1);
        
        if (feetCollider.IsTouchingLayers(climbLayer))
        {
            // Check if player wants to go up
            if (yAxis > Mathf.Epsilon)
                rb2d.velocity = new Vector2(rb2d.velocity.x, climbSpeed);
            
            // Check if player wants to go down
            else if (yAxis < -Mathf.Epsilon)
                rb2d.velocity = new Vector2(rb2d.velocity.x, -climbSpeed);
            
            // Else, player wants to stay still
            else
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);

            isClimbing = true;
        }

        // Player is not touching the ladder
        else
            isClimbing = false;

    }

    void CheckForJump()
    {   
        bool keyDown = Input.GetButtonDown("Jump") || touchInput.GetButtonDown("Jump");
        // Check if player is touching the ground
        if (keyDown && feetCollider.IsTouchingLayers(groundLayer))
        {
            // If the player is holding something, show a "nope" bubble and don't jump.
            if (heldItem != null) 
                speechBubble.ShowBubble("nope");

            // Otherwise, update player vertical velocity
            else
            {
                float newYVelocity = Mathf.Max(rb2d.velocity.y, jumpSpeed);
                rb2d.velocity = new Vector2(rb2d.velocity.x, newYVelocity);                
            }
        }
    }

    void Animate() 
    {
        // Flip sprite vertically if player is dead
        sprRen.flipY = isDead;

        // Flip sprite based on velocity
        float xVelocity = rb2d.velocity.x;
        if (xVelocity > Mathf.Epsilon)
            sprRen.flipX = false;
        else if (xVelocity < -Mathf.Epsilon)
            sprRen.flipX = true;

        // Change parameters based on whether player is touching the ground
        bool isInAir = !feetCollider.IsTouchingLayers(groundLayer);
        anim.SetBool("isInAir", isInAir);

        // Change parameters based on whether player is touching a climbable object
        bool isClimbing = heldItem == null && feetCollider.IsTouchingLayers(climbLayer);
        anim.SetBool("isClimbing", isClimbing);

        // Change parameters based on movement conditions
        bool isWalking = (Mathf.Abs(xVelocity) > Mathf.Epsilon);   // If player has horizontal velocity
        anim.SetBool("isWalking", isWalking);    
    }

    public void Die()
    {
        Debug.Log("Player died. Respawing...");
        StartCoroutine(DieCoroutine());
    }

    void CheckForGrabOrRelease()
    {
        bool keyDown = Input.GetButtonDown("Fire1") || touchInput.GetButtonDown("Fire1");
        if (keyDown)
        {
            // If player is empty-handed, try to grab the nearest item
            if (heldItem == null)
            {
                // If a nearby grabbable was found, grab it
                if (nearestGrabbable != null)
                    GrabItem(nearestGrabbable);
            }

            // Otherwise, release the item
            else
                ReleaseItem();
        }
    }

    void UpdateHeldItem()
    {
        // Ignore if no item is held
        if (heldItem == null) return;

        Vector3 target = transform.position + heldItemOffset;

        // If the item has been gripped on, move the item along with the player
        if (gripObtained)
        {
            heldItem.transform.position = target;
            heldItem.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        // Otherwise, have the object drift toward the player's grip
        else {
            Vector3 heldItemPosition = heldItem.transform.position;
            float step = (float)(7.5 * Time.deltaTime);
            
            heldItem.transform.position = Vector2.MoveTowards(heldItemPosition, target, step);

            // Check if the grip has been acquired
            float ItemAndGripDistance = Vector2.Distance(heldItem.transform.position, target);
            if (ItemAndGripDistance < 0.05)
                gripObtained = true;
        }

    }

    void GrabItem(Grabbable grabbable)
    {
        // Change animator parameter
        anim.SetBool("isCarrying", true);

        // Disable the outline of the item
        grabbable.GetComponent<LineRenderer>().enabled = false;

        heldItem = grabbable;

        // Disable the item's physics and collision
        Rigidbody2D itemRb2d = heldItem.GetComponent<Rigidbody2D>();
        itemRb2d.velocity = new Vector2(0, 0);
        itemRb2d.isKinematic = true;
        Collider2D itemCollider2D = heldItem.GetComponent<Collider2D>();
        itemCollider2D.enabled = false;

        // Update the player's mass
        rb2d.mass += itemRb2d.mass;
    }

    void ReleaseItem()
    {
        // Change animator parameter
        anim.SetBool("isCarrying", false);

        // Reenable the item's physics and collision
        Rigidbody2D itemRb2d = heldItem.GetComponent<Rigidbody2D>();
        itemRb2d.isKinematic = false;
        Collider2D itemCollider2D = heldItem.GetComponent<Collider2D>();
        itemCollider2D.enabled = true;

        // Give item velocity
        itemRb2d.velocity = rb2d.velocity;

        // Update the player's mass
        rb2d.mass -= itemRb2d.mass;

        // Reset state variables
        heldItem = null;
        gripObtained = false;
    }

    void UpdateGravityScale()
    {
        if (isClimbing)
            // Allow player to freely go up and down ladder w/o being affected by gravity
            rb2d.gravityScale = 0f; 
        else
            rb2d.gravityScale = 1f;
    }

    public IEnumerator DieCoroutine()
    {
        // Set isDead flag
        isDead = true;

        // Release the item the player is holding
        if (heldItem != null)
            ReleaseItem();

        // Wait 3 seconds
        yield return new WaitForSeconds(3);

        // Reload the level
        FindObjectOfType<SceneChanger>().ReloadScene();
    }

}
