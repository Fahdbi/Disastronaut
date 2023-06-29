using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScene : MonoBehaviour
{
    [SerializeField] float controlsDuration = 6f;

    void Start()
    {
        StartCoroutine(WaitAndLoadFirstLevel());
    }

    IEnumerator WaitAndLoadFirstLevel()
    {
        yield return new WaitForSeconds(controlsDuration);
        FindObjectOfType<SceneChanger>().LoadLevel(0);
    }
}
