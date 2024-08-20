using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;

    [Header("Managers")]
    public LevelLoader levelLoader;
    public MenuManager menuManager;

    private string gameOverLevel;
    private string mainMenuLevel;

    void Start()
    {
        gameOverLevel = "GameOver";
        mainMenuLevel = "MainMenu";

        // Ensure all panels are off except the main menu.
        menuManager.PanelSwitch(menuPanel);
    }

    public void ClickPlay()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        levelLoader.LoadScene(nextSceneIndex);
        menuPanel.SetActive(false);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverLevel);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(mainMenuLevel);
    }

}