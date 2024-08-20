using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ScaleState {
    None,
    Growing,
    Shrinking,
}

public class PlayerScalePower : MonoBehaviour
{
    public event Action<float> OnUpdateScalePoints;
    public event Action<ScaleState> OnChangeScaleState;
    [SerializeField] PlayerMode playerMode;
    [SerializeField] Material[] flashlightBeamMaterials;
    [SerializeField] Light flashlightLight;
    [SerializeField] Color baseColor;
    [SerializeField] Color growColor;
    [SerializeField] Color shrinkColor;
    [SerializeField] float startingScalePoints = 50f;
    [SerializeField] float maxScalePoints = 100f;

    [Tooltip("How many scale points are used with one second of growing/shrinking an object")]
    [SerializeField] float scalePointsPerSecond = 10f;

    [Tooltip("How many units is the object's scale modified by when 1 scale point is spent on it")]
    [SerializeField] float unitsScaledPerScalePoint = 0.1f;

    [Tooltip("The value that this curve evaluates to is applied as a multiplier to scalePointsPerSecond")]
    [SerializeField] AnimationCurve scaleRateCurve;

    [Tooltip("This is the amount of time represented by 1 on the scaleRateCurve")]
    [SerializeField] float scaleRateTime = 1f;

    float currentScalePoints;
    ScalableObject currObject;
    private PlayerRaycast playerRaycast;

    ScaleState state = ScaleState.None;
    float elapsedTimeScaling = 0f;

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

    private void Update()
    {
        if (playerMode.GetPlayerState() == PlayerState.Grab)
        {
            if (state != ScaleState.None)
            {
                OnChangeScaleState(ScaleState.None);
            }
            state = ScaleState.None;
            HandleFlashlightColor();
            elapsedTimeScaling = 0f;
            return;
        }
        if (currObject == null || state == ScaleState.None)
        {
            elapsedTimeScaling = 0f;
            return;
        }
        
        float curveFactor = scaleRateCurve.Evaluate(Mathf.Clamp01(elapsedTimeScaling / scaleRateTime));

        if (state == ScaleState.Growing)
        {
            if (currentScalePoints <= 0f || !currObject.CheckIfCanGrow()) return;
            float pointsSpent = Mathf.Min(scalePointsPerSecond * Time.deltaTime * curveFactor, maxScalePoints - currentScalePoints);
            currObject.Grow(pointsSpent * unitsScaledPerScalePoint, transform.position);
            // Debug.Log($"pointsSpend: {pointsSpent}, deltaTime: {Time.deltaTime}, max: {maxScalePoints - currentScalePoints}, growthAmount: {pointsSpent * unitsScaledPerScalePoint}");
            currentScalePoints -= pointsSpent;
            currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, maxScalePoints);
            OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
            elapsedTimeScaling += Time.deltaTime;
        } else if (state == ScaleState.Shrinking)
        {
            if (currentScalePoints >= maxScalePoints) return;
            // This is the most points that can be spent on this object before it would reduce it below it's minimum scale
            float mostPointsSpendable = (currObject.GetCurrentScale() - currObject.GetMinimumScale()) / unitsScaledPerScalePoint;
            float pointsSpent = Mathf.Min(scalePointsPerSecond * Time.deltaTime * curveFactor, currentScalePoints, mostPointsSpendable);
            currObject.Shrink(pointsSpent * unitsScaledPerScalePoint, transform.position);
            // Debug.Log($"pointsSpend: {pointsSpent}, shrinkAmount: {pointsSpent * unitsScaledPerScalePoint}");
            currentScalePoints += pointsSpent;
            currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, maxScalePoints);
            OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
            elapsedTimeScaling += Time.deltaTime;
        }
    }

    private void HandleMouseOverScalableObject(ScalableObject scalableObject)
    {

        if (scalableObject != null)
        {
            currObject = scalableObject;
            //Debug.Log("Mouse is over a scalable object: " + scalableObject.name);
        }
        else
        {
            currObject = null;
            //Debug.Log("Mouse is not over any scalable object.");
        }
    }

    public void OnGrowObject(InputValue value)
    {
        if (playerMode.GetPlayerState() == PlayerState.Grab || state == ScaleState.Shrinking) return;
        ScaleState newState = value.isPressed ? ScaleState.Growing : ScaleState.None;
        if (newState != state)
        {
            OnChangeScaleState?.Invoke(newState);
        }
        state = newState;
        HandleFlashlightColor();
    }

    public void OnShrinkObject(InputValue value)
    {
        if (playerMode.GetPlayerState() == PlayerState.Grab || state == ScaleState.Growing) return;
        ScaleState newState = value.isPressed ? ScaleState.Shrinking : ScaleState.None;
        if (newState != state)
        {
            OnChangeScaleState?.Invoke(newState);
        }
        state = newState;
        HandleFlashlightColor();
    }

    void HandleFlashlightColor()
    {
        Color color = Color.white;
        switch(state)
        {
            case ScaleState.None:
                color = baseColor;
                break;
            case ScaleState.Growing:
                color = growColor;
                break;
            case ScaleState.Shrinking:
                color = shrinkColor;
                break;
        }

        foreach(Material material in flashlightBeamMaterials)
        {
            material.SetVector("_Beam_Color", color);
        }
        flashlightLight.color = color;
    }

    public ScaleState GetState()
    {
        return state;
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
