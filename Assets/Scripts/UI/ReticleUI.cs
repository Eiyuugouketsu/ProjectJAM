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
    Push,
    Blocked
}

public class ReticleUI : MonoBehaviour
{
    ReticleState state;
    [SerializeField] PlayerRaycast playerRaycast;
    [SerializeField] PlayerMode playerMode;
    [SerializeField] PlayerGrabAbility playerGrabAbility;
    [SerializeField] PlayerScalePower playerScalePower;
    [SerializeField] Image reticleImage;
    
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite wSprite;
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

    private void Start()
    {
        playerRaycast.OnMouseOverScalableObject += Player_OnMouseOverScalableObject;
        playerGrabAbility.OnObjectPickedUp += PlayerGrabAbility_OnObjectPickedUp;
        playerGrabAbility.OnObjectDropped += PlayerGrabAbility_OnObjectDropped;
        playerScalePower.OnUpdateScalePoints += PlayerScalePower_OnUpdateScalePoints;
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
        //Debug.Log($"newState: {newState}");
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
            case ReticleState.Push:
                reticleImage.sprite = wSprite;
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
        // If player is holding an object do nothing
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
                    return;
                }

                if (objectMass <= pushThreshold)
                {
                    SetReticleState(ReticleState.Push);
                    return;
                }
            } else
            {
                float currentScalePoints = playerScalePower.GetCurrentScalePoints();
                bool canShrink = currentScalePoints > 0 && currentTarget.transform.localScale.x > currentTarget.GetMinimumScale();
                bool canGrow = currentScalePoints < playerScalePower.GetMaxScalePoints();

                if (canShrink && canGrow) SetReticleState(ReticleState.Both);
                else if (!canShrink && canGrow) SetReticleState(ReticleState.Left);
                else if (canShrink && !canGrow) SetReticleState(ReticleState.Right);
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
}
