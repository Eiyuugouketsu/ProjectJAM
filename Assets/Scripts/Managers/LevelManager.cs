using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentLevelId = 0;
    public bool LevelCompleted;
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
        if(Input.GetKeyDown(KeyCode.O))
        {
            LevelCompleted = true;
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
        while(currentLevelId +1 < 10)
        {
            yield return new WaitUntil(() => LevelCompleted);
            LevelCompleted = false;
            currentLevelId ++;
            LoadNextLevel();
        }
    }
}
