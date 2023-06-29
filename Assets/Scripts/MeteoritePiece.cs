using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritePiece : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [Range(0, 60)]
    [SerializeField] float minLifespan = 2.5f;
    [Range(0, 60)]
    [SerializeField] float maxLifespan = 7.5f;
    
    float lifespan = 0;
    float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
        SetLifespan();
        UseSprite();
        SetVelocity();
    }

    void Update()
    {
        // If its lifespan has been exceeded
        if (lifespan > 0 && (Time.time - spawnTime) > lifespan)
        {
            // Destroy the meteorite piece 
            Destroy(gameObject);
        }
    }

    void SetLifespan()
    {
        lifespan = Random.Range(minLifespan, maxLifespan);
    }

    void UseSprite()
    {
        SpriteRenderer sprRen = GetComponent<SpriteRenderer>();

        // Choose a random sprite and use it
        Sprite sprite = sprites[Random.Range(0, sprites.Length)];
        sprRen.sprite = sprite;
    }

    void SetVelocity()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        // Give the piece a random velocity
        Vector3 direction = Random.insideUnitCircle;
        Vector3 velocity = new Vector3(direction.x, Mathf.Abs(direction.y), direction.z) * 5;
        rb2d.velocity = velocity;
    }

}
