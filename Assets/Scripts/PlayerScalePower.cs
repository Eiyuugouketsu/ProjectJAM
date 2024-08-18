using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalePower : MonoBehaviour
{
    public event Action<float> OnUpdateScalePoints;
    [SerializeField] float startingScalePoints = 50f;
    [SerializeField] float maxScalePoints = 100f;
    [SerializeField] float scaleFactor = 100f;
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

        OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
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

    private void FixedUpdate()
    {
        
    }

    public void OnGrowObject()
    {
        if (currObject == null || currentScalePoints >= maxScalePoints) return;
        float growAmount = Mathf.Min(Time.deltaTime * scaleFactor, maxScalePoints - currentScalePoints);
        currObject.Grow(growAmount / scaleFactor);
        currentScalePoints += growAmount;
        currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, maxScalePoints);
        OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
    }

    public void OnShrinkObject()
    {
        if (currObject == null || currentScalePoints <= 0f) return;
        float shrinkAmount = Mathf.Min(Time.deltaTime * scaleFactor, currentScalePoints);
        currObject.Shrink(shrinkAmount / scaleFactor);
        currentScalePoints -= shrinkAmount;
        currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, maxScalePoints);
        OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
    }

    public float GetCurrentScalePoints()
    {
        return currentScalePoints;
    }

    public float GetMaxScalePoints()
    {
        return maxScalePoints;
    }
}
