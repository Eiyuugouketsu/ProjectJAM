using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentLevelId = 1;
    public bool LevelCompleted = false;
    public void StartGame()
    {
        StartCoroutine(Process());
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
        VOTrigger.audioQueue.Clear();
        SceneManager.LoadScene(currentLevelId);
    }
    IEnumerator Process()
    {
        while(currentLevelId < 3)
        {
            yield return new WaitUntil(() => LevelCompleted);
            LevelCompleted = false;
            currentLevelId ++;
            LoadNextLevel();
            Debug.Log("test1");
        }
        Debug.Log("test");
    }
}
