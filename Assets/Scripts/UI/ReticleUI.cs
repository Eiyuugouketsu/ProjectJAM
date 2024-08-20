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
    [SerializeField] Image clickReticleImage;
    
    [SerializeField] Sprite defaultSprite;
    [SerializeField] float defaultSpriteScale;
    [SerializeField] Sprite leftClickSprite;
    [SerializeField] Sprite rightClickSprite;
    [SerializeField] Sprite blockedSprite;
    [SerializeField] float clickSpriteScale;

// TODO: Move these to a singleton or global variable container
    [SerializeField] float pushThreshold;
    [SerializeField] float pickupThreshold;
    [SerializeField] float throwThreshold;

    float bothClickTimer = 0f;
    [SerializeField] float bothClickAlternationTime = 0.5f;

    ScalableObject currentTarget;

    void Start()
    {
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
            clickReticleImage.sprite = clickReticleImage.sprite == leftClickSprite ? rightClickSprite : leftClickSprite;
            bothClickTimer = 0f;
        }
    }

    private void SetBothClickState()
    {
        clickReticleImage.sprite = leftClickSprite;
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
                reticleImage.enabled = false;
                clickReticleImage.enabled = true;
                break;
            case ReticleState.Left:
                reticleImage.enabled = false;
                clickReticleImage.enabled = true;
                clickReticleImage.sprite = leftClickSprite;
                break;
            case ReticleState.Right:
                reticleImage.enabled = false;
                clickReticleImage.enabled = true;
                clickReticleImage.sprite = rightClickSprite;
                break;
            case ReticleState.Blocked:
                reticleImage.enabled = false;
                clickReticleImage.enabled = true;
                clickReticleImage.sprite = blockedSprite;
                break;
            default:
                reticleImage.enabled = true;
                clickReticleImage.enabled = false;
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
            if (!currentTarget.GetIsInteractable())
            {
                SetReticleState(ReticleState.Blocked);
                return;
            }

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

                // Debug.Log($"canShrink: {canShrink}, canGrow: {canGrow}, isTryingToGrow: {isTryingToGrow}, isTryingToShrink: {isTryingToShrink}");

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
