using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<Level> Levels = new List<Level>();
    int currentLevelId = 0;
    [SerializeField] TransitionBox transitionBoxPrefab;
    TransitionBox transitionBox;
    Level activeLevel;
    public bool LevelCompleted;
    public bool TransitionBoxExited;

    public void StartGame(int levelId)
    {
        StartCoroutine(Process(levelId));
    }

    public void ResetLevel()
    {
        Destroy(PlayerThresholds.Instance.gameObject);
        Destroy(activeLevel);
        Destroy(transitionBox);
        StopAllCoroutines();
        StartGame(currentLevelId);
    } 

    IEnumerator LoadLevel(int levelId)
    {
        Level level = Instantiate(Levels[levelId]);
        yield return new WaitUntil(() => level.isActiveAndEnabled);
        activeLevel = level;
        transitionBox = Instantiate(transitionBoxPrefab,level.TransitionBoxSpawn.transform.position, new Quaternion());
        yield return new WaitUntil(() => transitionBox.isActiveAndEnabled);
        var Player = Instantiate(playerPrefab, level.PlayerSpawnPoint.position, new Quaternion());
    }

    IEnumerator LoadNextLevel()
    {
        Level level = Instantiate(Levels[currentLevelId],transitionBox.levelLoadPoint.position,new Quaternion());
        yield return new WaitUntil(() => level.isActiveAndEnabled);
        activeLevel = level;
    }
    IEnumerator Process(int levelId)
    {
        currentLevelId = levelId;
        yield return LoadLevel(levelId);
        GameManager.Instance.playerCamera.Follow = PlayerThresholds.Instance.cam;
        GameManager.Instance.UIManager.PlayerSpawn();
        while(currentLevelId +1 < Levels.Count)
        {
            yield return new WaitUntil(() => LevelCompleted);
            LevelCompleted = false;
            currentLevelId ++;
            activeLevel.DisableAndDestroy();
            yield return LoadNextLevel();
            yield return new WaitUntil(() => TransitionBoxExited);
            transitionBox.DisableAndDestroy();
            TransitionBoxExited = false;
            transitionBox = Instantiate(transitionBoxPrefab,activeLevel.TransitionBoxSpawn.transform.position,new Quaternion());
        }
    }
}
