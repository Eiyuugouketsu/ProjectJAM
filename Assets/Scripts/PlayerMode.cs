using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum PlayerState
{
    Grab,
    Scale
}

public class PlayerMode : MonoBehaviour
{

    public PlayerState currentState = PlayerState.Grab;
    private PlayerScalePower playerScalePower;
    private PlayerGrabAbility playerGrabAbility;

    [SerializeField] Transform flashlightObjectTransform;
    [SerializeField] Transform flashlightActiveTransform;
    [SerializeField] Transform flashlightInactiveTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerScalePower = GetComponent<PlayerScalePower>();
        playerGrabAbility = GetComponent<PlayerGrabAbility>();
        UpdateState();
    }

    public void OnChangeMode()
    {
        if (currentState == PlayerState.Grab)
        {
            currentState = PlayerState.Scale;
            Debug.Log("Switched to Scale State");
        }
        else if (currentState == PlayerState.Scale)
        {
            currentState = PlayerState.Grab;
            Debug.Log("Switched to Grab State");
        }
        UpdateState();
    }

    void UpdateState()
    {
        if (currentState == PlayerState.Grab)
        {
            playerGrabAbility.enabled = true;
            playerScalePower.enabled = false;
            flashlightObjectTransform.localPosition = flashlightInactiveTransform.localPosition;
            flashlightObjectTransform.localRotation = flashlightInactiveTransform.localRotation;
        }
        else if (currentState == PlayerState.Scale)
        {
            playerGrabAbility.enabled = false;
            playerScalePower.enabled = true;
            flashlightObjectTransform.localPosition = flashlightActiveTransform.localPosition;
            flashlightObjectTransform.localRotation = flashlightActiveTransform.localRotation;
        }
    }

    public PlayerState GetPlayerState()
    {
        return currentState;
    }

}
