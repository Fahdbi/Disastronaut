using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [Header("Open Period")]
    [Range(3, 10)]
    [SerializeField] float minOpenPeriod = 4;
    
    [Range(3, 10)]
    [SerializeField] float maxOpenPeriod = 8;

    [Header("Close Period")]
    [Range(3, 10)]
    [SerializeField] float minClosePeriod = 4;
    
    [Range(3, 10)]
    [SerializeField] float maxClosePeriod = 8;

    Animator anim;
    PolygonCollider2D polyCol2D;
    SpriteRenderer sprRen;

    void Start()
    {
        anim = GetComponent<Animator>();
        polyCol2D = GetComponent<PolygonCollider2D>();
        sprRen = GetComponent<SpriteRenderer>(); 

        StartCoroutine(StartCycle());  
    }

    
    IEnumerator StartCycle() {
        while (true)
        {
            // Close bridge
            anim.SetBool("isOpen", false);
            float closePeriod = Random.Range(minClosePeriod, maxClosePeriod);
            yield return new WaitForSeconds(closePeriod);

            // Open bridge
            anim.SetBool("isOpen", true);
            float openPeriod = Random.Range(minOpenPeriod, maxOpenPeriod);
            yield return new WaitForSeconds(openPeriod);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColliderShape();
    }

    private void UpdateColliderShape()
    {
        List<Vector2> physicsShape = new List<Vector2>();
        sprRen.sprite.GetPhysicsShape(0, physicsShape);
        polyCol2D.points = physicsShape.ToArray();
    }

}
