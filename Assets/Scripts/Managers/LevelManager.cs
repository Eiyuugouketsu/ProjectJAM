using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentLevelId = 0;
    public bool LevelCompleted;

    private void Awake() 
    {
        
    }

    public void StartGame(int levelId)
    {
        StartCoroutine(Process(levelId));
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ResetLevel();
        }
    }

    void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevelId);
    }
    IEnumerator Process(int levelId)
    {
        currentLevelId = levelId;
        LoadLevel(levelId);
        while(currentLevelId +1 < 5)
        {
            yield return new WaitUntil(() => LevelCompleted);
            LevelCompleted = false;
            currentLevelId ++;
            LoadNextLevel();
        }
    }
}
