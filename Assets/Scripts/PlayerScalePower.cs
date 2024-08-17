using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalePower : MonoBehaviour
{
    public event Action<float> OnUpdateScalePoints;
    [SerializeField] float startingScalePoints = 50f;
    float currentScalePoints;
    
    private void Awake()
    {
        currentScalePoints = startingScalePoints;
    }

    private void Start()
    {
        OnUpdateScalePoints?.Invoke(currentScalePoints / 100f);
    }

    private void Update()
    {
        Debug_GrowShrink();
    }

    // Temporary method to change scale points for testing
    private void Debug_GrowShrink()
    {
        if (Input.GetMouseButton(0) && currentScalePoints < 100f)
        {
            currentScalePoints += Time.deltaTime * 15f;
        } else if (Input.GetMouseButton(1) && currentScalePoints > 0f)
        {
            currentScalePoints -= Time.deltaTime * 15f;
        }

        currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, 100f);
        OnUpdateScalePoints?.Invoke(currentScalePoints / 100f);
    }
}
