using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] MeteoritePiece meteoritePiecePrefab;

    [SerializeField] Vector3 velocity = new Vector3(0, 0, 0);
    [SerializeField] float lifespan = 0;
    
    [SerializeField] float pieceLifespan = 0;
    [SerializeField] int numOfPieces = 20;
    [SerializeField] LayerMask groundLayer;

    float spawnTime;
    CircleCollider2D col;

    void Start()
    {
        spawnTime = Time.time;
        col = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        // If its lifespan has been exceeded
        if (lifespan > 0 && (Time.time - spawnTime) > lifespan)
        {
            // Destroy the meteorite 
            Destroy(gameObject);
        }

        // Else, if it has hit the ground
        else if (col.IsTouchingLayers(groundLayer))
        {
            SpawnPieces();

            // Destroy the meteorite 
            Destroy(gameObject);
        }

        transform.position += velocity * Time.deltaTime;
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    public void SetLifespan(float ls)
    {
        lifespan = ls;
    }

    // Spawn meteorite pieces in its place
    private void SpawnPieces()
    {
        // Spawn the pieces
        float colRadius = col.radius;
        for (int i = 0; i < numOfPieces; i++)  
        {
            // Position: inside the meteorite
            Vector2 offset = Random.insideUnitCircle * colRadius;
            Vector3 piecePosition = transform.position + new Vector3(offset.x, offset.y);
            Instantiate(meteoritePiecePrefab, piecePosition, Quaternion.identity);
        }
    }

    // Kill player if player collides with this game object
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObject = other.gameObject;

        // Check if the other object is a Player     
        if (otherObject.GetComponent<Player>() != null)
            otherObject.GetComponent<Player>().Die();    // Tell player to die   
    }

}
