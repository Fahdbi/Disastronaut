using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] GameObject cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float zPosition = transform.position.z;
        transform.position = new Vector3(
            cam.transform.position.x, 
            cam.transform.position.y,
            zPosition
        );
    }
}
