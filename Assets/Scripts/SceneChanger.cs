using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Awake() {
        // Enable the black curtain, which is disabled and hidden by default in the scene view
        GetComponentInChildren<Image>().enabled = true;       
    }
 
    public void StartGame()
    {
        SceneManager.LoadScene("Controls");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }   
    public void LoadLevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }    
    
    public void LoadTheEnd()
    {
        SceneManager.LoadScene("The End");
    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene("Level " + levelNum);
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // Check if current scene is a level
        if (currentSceneName.Substring(0, 6) != "Level ")
        {
            Debug.Log("Cannot load next level because the current scene is not a level.");
            return;
        }

        int levelNum = int.Parse(currentSceneName.Substring(6)) + 1;
        LoadLevel(levelNum);
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Quit()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}
