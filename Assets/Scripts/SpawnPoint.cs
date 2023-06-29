using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    void Start()
    {
        // Set player position to the spawn point's position
        Player player = FindObjectOfType<Player>();
        if (player == null) 
            Debug.LogError("Cannot instantiate player position because no player was found.");
        else
            player.transform.position = transform.position;
    }

}
