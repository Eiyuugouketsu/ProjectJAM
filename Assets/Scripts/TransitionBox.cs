using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBox : MonoBehaviour
{
    [SerializeField] Trigger transitionTrigger;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] DoorController doorController;
    [SerializeField] float fadeTime = 2f;
    [SerializeField] float afterFadeTime = 2f;

    float afterFadeTimer = 0f;

    void OnEnable()
    {
        transitionTrigger.OnEventTriggerEnter += TransitionToNextScene;
    }

    void TransitionToNextScene(Collider other)
    {
        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        if(doorController != null)
        {
            doorController.ClosingState();
        }
        audioSource.Play();
        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeTime;
            yield return null;
        }
        while (afterFadeTimer < afterFadeTime)
        {
            afterFadeTimer += Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.levelManager.LevelCompleted = true;
        Destroy(transitionTrigger.gameObject);
    }

    public void DisableAndDestroy()
    {
        Destroy(gameObject);
    }
}
