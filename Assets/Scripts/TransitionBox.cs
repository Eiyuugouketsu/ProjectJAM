using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBox : MonoBehaviour
{
    [SerializeField] Trigger closeEnterDoorTrigger;
    [SerializeField] Trigger openExitDoorTrigger;
    [SerializeField] Trigger exitTransitionBoxTrigger;
    [SerializeField] public Transform levelLoadPoint;
    void OnEnable()
    {
        closeEnterDoorTrigger.OnEventTriggerEnter += CloseDoor;
        openExitDoorTrigger.OnEventTriggerEnter += OpenDoor;
        exitTransitionBoxTrigger.OnEventTriggerEnter += ExitTranitionBox;
    }

    void CloseDoor(Collider other)
    {
        GameManager.Instance.levelManager.LevelCompleted = true;
        Destroy(closeEnterDoorTrigger);
    }

    void OpenDoor(Collider other)
    {

    }

    void ExitTranitionBox(Collider other)
    {
        GameManager.Instance.levelManager.TransitionBoxExited = true;
    }

    public void DisableAndDestroy()
    {
        Destroy(gameObject);
    }
}
