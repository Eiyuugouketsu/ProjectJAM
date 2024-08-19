using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ReticleState {
    Default,
    Left,
    Right,
    Both,
    Blocked
}

public class ReticleUI : MonoBehaviour
{
    ReticleState state;
    [SerializeField] PlayerRaycast playerRaycast;
    PlayerMode playerMode;
    PlayerGrabAbility playerGrabAbility;
    PlayerScalePower playerScalePower;
    [SerializeField] Image reticleImage;
    
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite leftClickSprite;
    [SerializeField] Sprite rightClickSprite;
    [SerializeField] Sprite blockedSprite;

// TODO: Move these to a singleton or global variable container
    [SerializeField] float pushThreshold;
    [SerializeField] float pickupThreshold;
    [SerializeField] float throwThreshold;

    float bothClickTimer = 0f;
    [SerializeField] float bothClickAlternationTime = 0.5f;

    ScalableObject currentTarget;

    void Start()
    {
        // Remove once we have the level manager working
        SubscribeToPlayerEvents();
    }

    public void SubscribeToPlayerEvents()
    {
        Debug.Log(PlayerThresholds.Instance.PlayerRaycast);
        PlayerThresholds.Instance.PlayerRaycast.OnMouseOverScalableObject += Player_OnMouseOverScalableObject;
        PlayerThresholds.Instance.PlayerRaycast.OnMouseOverGrabbableObject += Player_OnMouseOverScalableObject;
        PlayerThresholds.Instance.PlayerMode.OnChangePlayerState += Player_OnChangePlayerState;
        PlayerThresholds.Instance.PlayerGrabAbility.OnObjectPickedUp += PlayerGrabAbility_OnObjectPickedUp;
        PlayerThresholds.Instance.PlayerGrabAbility.OnObjectDropped += PlayerGrabAbility_OnObjectDropped;
        PlayerThresholds.Instance.PlayerScalePower.OnUpdateScalePoints += PlayerScalePower_OnUpdateScalePoints;
        PlayerThresholds.Instance.PlayerScalePower.OnChangeScaleState += PlayerScalePower_OnChangeScaleState;
        playerGrabAbility = PlayerThresholds.Instance.PlayerGrabAbility;
        playerScalePower = PlayerThresholds.Instance.PlayerScalePower;
        playerMode = PlayerThresholds.Instance.PlayerMode;
        reticleImage.sprite = defaultSprite;
    }

    private void Update()
    {
        if (state != ReticleState.Both) return;
        bothClickTimer += Time.deltaTime;
        if (bothClickTimer >= bothClickAlternationTime)
        {
            reticleImage.sprite = reticleImage.sprite == leftClickSprite ? rightClickSprite : leftClickSprite;
            bothClickTimer = 0f;
        }
    }

    private void SetBothClickState()
    {
        reticleImage.sprite = leftClickSprite;
        bothClickTimer = 0f;
    }

    private void SetReticleState(ReticleState newState)
    {
        // Debug.Log($"newState: {newState}");
        if (state != ReticleState.Both && newState == ReticleState.Both) SetBothClickState();
        state = newState;

        switch (newState)
        {
            case ReticleState.Both:
                break;
            case ReticleState.Left:
                reticleImage.sprite = leftClickSprite;
                break;
            case ReticleState.Right:
                reticleImage.sprite = rightClickSprite;
                break;
            case ReticleState.Blocked:
                reticleImage.sprite = blockedSprite;
                break;
            default:
                reticleImage.sprite = defaultSprite;
                break;
        }
    }

    private void CheckForReticleState()
    {
        if (playerGrabAbility.GetCurrentObject() != null && playerGrabAbility.GetIsHoldingObject())
        {
            float objectMass = playerGrabAbility.GetCurrentObject().GetMass();
            if (objectMass <= throwThreshold)
            {
                SetReticleState(ReticleState.Both);
            } else
            {
                SetReticleState(ReticleState.Right);
            }
            return;
        }

        if (currentTarget != null)
        {
            if (playerMode.GetPlayerState() == PlayerState.Grab)
            {
                float objectMass = currentTarget.GetMass();
                if (objectMass <= pickupThreshold)
                {
                    SetReticleState(ReticleState.Right);
                } else {
                    SetReticleState(ReticleState.Blocked);
                }
                return;
            } else
            {
                float currentScalePoints = playerScalePower.GetCurrentScalePoints();
                bool canShrink = currentScalePoints > 0 && currentTarget.transform.localScale.x > currentTarget.GetMinimumScale();
                bool canGrow = currentScalePoints < playerScalePower.GetMaxScalePoints();

                bool isTryingToGrow = playerScalePower.GetState() == ScaleState.Growing;
                bool isTryingToShrink = playerScalePower.GetState() == ScaleState.Shrinking;

                if ((isTryingToGrow && !canGrow) || (isTryingToShrink && !canShrink)) SetReticleState(ReticleState.Blocked);
                else if (canShrink && canGrow) SetReticleState(ReticleState.Both);
                else if (!canShrink && canGrow) SetReticleState(ReticleState.Right);
                else if (canShrink && !canGrow) SetReticleState(ReticleState.Left);
                else if (!canGrow && !canShrink) SetReticleState(ReticleState.Blocked);
                return;
            }
        }

        SetReticleState(ReticleState.Default);
    }

    private void Player_OnMouseOverScalableObject(ScalableObject scalableObject)
    {
        currentTarget = scalableObject;
        CheckForReticleState();
    }

    private void PlayerGrabAbility_OnObjectPickedUp(ScalableObject scalableObject)
    {
        currentTarget = scalableObject;
        CheckForReticleState();
    }

    private void PlayerGrabAbility_OnObjectDropped()
    {
        CheckForReticleState();
    }

    private void PlayerScalePower_OnUpdateScalePoints(float value)
    {
        CheckForReticleState();
    }

    private void PlayerScalePower_OnChangeScaleState(ScaleState newState)
    {
        CheckForReticleState();
    }

    private void Player_OnChangePlayerState(PlayerState newState)
    {
        CheckForReticleState();
    }
}
