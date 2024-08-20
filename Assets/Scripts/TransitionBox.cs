using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBox : MonoBehaviour
{
    [SerializeField] Trigger transitionTrigger;
    void OnEnable()
    {
        transitionTrigger.OnEventTriggerEnter += TransitionToNextScene;
    }

    void TransitionToNextScene(Collider other)
    {
        GameManager.Instance.levelManager.LevelCompleted = true;
        Destroy(transitionTrigger.gameObject);
    }

    public void DisableAndDestroy()
    {
        Destroy(gameObject);
    }
}
