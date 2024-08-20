using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{   
    public MenuManager menuManager;
    //public GameObject loadingCanvas;

    public void StartLevelAnimation()
    {

    }

    public void LoadScene(int scene){
        Debug.Log("Next scene index: " + scene);
        //menuManager.PanelSwitch(loadingCanvas);
        StartCoroutine(LoadAsynchronously(scene));
	}

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        yield return new WaitForSeconds(3);
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        loading.allowSceneActivation = false;

        // Play fade animation
        while (!loading.isDone)
        {
            if (loading.progress >= 0.9f)
            {
                //loadingCanvas.SetActive(false);
                GameManager.Instance.levelManager.StartGame();
                loading.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }

    public void ClickPlay()
    {
        // Get the next scene index and then load it async.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        LoadScene(nextSceneIndex);
    }

    // TODO: Remove this in the build.
    private void Update() {
        if (Input.GetKeyDown (KeyCode.P))
        {
            Debug.Log("Changing scene");
            ClickPlay();
        }
    }

}
