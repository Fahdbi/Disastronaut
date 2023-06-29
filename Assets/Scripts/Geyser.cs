using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    [Header("Eruptions")]
    [Range(3, 20)]
    [SerializeField] float minPeriod = 4;
    
    [Range(3, 20)]
    [SerializeField] float maxPeriod = 8;

    [Range(0, 2)]
    [SerializeField] float particleDuration = 0.5f;

    [Header("Forces on objects")]
    float minAngularVelocity = -90f;
    float maxAngularVelocity = 90f;
    
    [Range(0, 20)]
    float verticalVelocity = 6f;

    Animator animator;
    BoxCollider2D boxCollider;
    ParticleSystem particleSys;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        particleSys = GetComponent<ParticleSystem>();
        
        StartCoroutine(StartCycle());
    }

    IEnumerator StartCycle() {
        while (true) 
        {
            // Wait to erupt
            float secondsToWait = minPeriod + (Random.value * (maxPeriod - minPeriod));
            yield return new WaitForSeconds(secondsToWait - particleDuration);

            // Start particle emitter
            particleSys.Play();
            yield return new WaitForSeconds(particleDuration);

            // Begin animating the eruption animation
            particleSys.Stop();
            animator.SetTrigger("erupt");
        }
    }

    // Enables the geyser's trigger
    public void EnableTrigger() {
        boxCollider.enabled = true;
    }

    // Disables the geyser's trigger
    public void DisableTrigger() {
        boxCollider.enabled = false;
    }

    // Kill player if player collides with this game object
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObject = other.gameObject;  

        // Throw object in the air
        Rigidbody2D rb2d = otherObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, verticalVelocity);
            rb2d.angularVelocity += Random.Range(minAngularVelocity, maxAngularVelocity);
        }

        // Check if the other object is a Player
        Player playerComponent = otherObject.GetComponent<Player>();
        if (playerComponent != null)
            playerComponent.Die();    // Tell player to die
    }

}
