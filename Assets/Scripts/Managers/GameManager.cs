using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public LevelManager levelManager;
    public UIManager UIManager;
    public MenuManager MenuManager; 
    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(levelManager.gameObject);
        DontDestroyOnLoad(UIManager.gameObject);
    }

    public void StartGame()
    {
        levelManager.StartGame();
    }
}
