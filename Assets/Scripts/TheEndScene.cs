using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEndScene : MonoBehaviour
{
    [SerializeField] float controlsDuration = 5f;

    void Start()
    {
        StartCoroutine(WaitAndLoadMainMenu());
    }

    IEnumerator WaitAndLoadMainMenu()
    {
        yield return new WaitForSeconds(controlsDuration);
        FindObjectOfType<SceneChanger>().LoadMainMenu();
    }
}
