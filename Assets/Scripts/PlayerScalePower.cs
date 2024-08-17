using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalePower : MonoBehaviour
{
    public event Action<float> OnUpdateScalePoints;
    [SerializeField] float startingScalePoints = 50f;
    float currentScalePoints;
    TestScale currObject;
    private PlayerRaycast playerRaycast;

    private void Awake()
    {
        currentScalePoints = startingScalePoints;
    }

    private void Start()
    {
        playerRaycast = GetComponent<PlayerRaycast>();

        if (playerRaycast != null)
        {
            playerRaycast.OnMouseOverScalableObject += HandleMouseOverScalableObject;
        }

        OnUpdateScalePoints?.Invoke(currentScalePoints / 100f);
    }

    private void HandleMouseOverScalableObject(TestScale scalableObject)
    {

        if (scalableObject != null)
        {
            currObject = scalableObject;
            Debug.Log("Mouse is over a scalable object: " + scalableObject.name);
        }
        else
        {
            currObject = null;
            Debug.Log("Mouse is not over any scalable object.");
        }
    }

    private void Update()
    {
        if (currObject != null) { 
            GrowShrink();
        }
    }

    private void FixedUpdate()
    {
        
    }

    // Temporary method to change scale points for testing
    private void GrowShrink()
    {
            if (Input.GetMouseButton(0) && currentScalePoints < 100f)
            {
                currObject.Grow();
                currentScalePoints += Time.deltaTime * 15f;
            }
            else if (Input.GetMouseButton(1) && currentScalePoints > 0f)
            {
                currObject.Shrink();
                currentScalePoints -= Time.deltaTime * 15f;
            } 
            else
            {
                currObject.Stop();
            }
            
            currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, 100f);
            OnUpdateScalePoints?.Invoke(currentScalePoints / 100f);
    }
}
