using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] PlayerRaycast playerRaycast;
    [SerializeField] Image reticleImage;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite pushableSprite;
    [SerializeField] Sprite pickupableSprite;
    [SerializeField] Sprite interactableSprite;

// TODO: Move these to a singleton or global variable container
    [SerializeField] float pushThreshold;
    [SerializeField] float pickupThreshold;

    private void Start()
    {
        playerRaycast.OnMouseOverScalableObject += Player_OnMouseOverScalableObject;
        reticleImage.sprite = defaultSprite;
    }

    private void Player_OnMouseOverScalableObject(TestScale scalableObject)
    {
        if (scalableObject != null)
        {
            float objectMass = scalableObject.GetMass();
            if (objectMass <= pickupThreshold)
            {
                reticleImage.sprite = pickupableSprite;
                return;
            }

            if (objectMass <= pushThreshold)
            {
                reticleImage.sprite = pushableSprite;
                return;
            }
        }

        reticleImage.sprite = defaultSprite;
    }
}
