using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public LevelManager levelManager;
    public UIManager UIManager;
    public CinemachineVirtualCamera playerCamera;
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
    }

    public void Start()
    {
        levelManager.StartGame(0);
    }
}
