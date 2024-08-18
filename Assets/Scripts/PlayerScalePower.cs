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
    [SerializeField] PlayerMode playerMode;
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
            state = ScaleState.None;
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
            if (currentScalePoints >= maxScalePoints || !currObject.CheckIfCanGrow()) return;
            float pointsSpent = Mathf.Min(scalePointsPerSecond * Time.deltaTime * curveFactor, maxScalePoints - currentScalePoints);
            currObject.Grow(pointsSpent * unitsScaledPerScalePoint);
            // Debug.Log($"pointsSpend: {pointsSpent}, deltaTime: {Time.deltaTime}, max: {maxScalePoints - currentScalePoints}, growthAmount: {pointsSpent * unitsScaledPerScalePoint}");
            currentScalePoints += pointsSpent;
            currentScalePoints = Mathf.Clamp(currentScalePoints, 0f, maxScalePoints);
            OnUpdateScalePoints?.Invoke(currentScalePoints / maxScalePoints);
            elapsedTimeScaling += Time.deltaTime;
        } else if (state == ScaleState.Shrinking)
        {
            if (currentScalePoints <= 0f) return;
            // This is the most points that can be spent on this object before it would reduce it below it's minimum scale
            float mostPointsSpendable = (currObject.GetCurrentScale() - currObject.GetMinimumScale()) / unitsScaledPerScalePoint;
            float pointsSpent = Mathf.Min(scalePointsPerSecond * Time.deltaTime * curveFactor, currentScalePoints, mostPointsSpendable);
            currObject.Shrink(pointsSpent * unitsScaledPerScalePoint);
            // Debug.Log($"pointsSpend: {pointsSpent}, shrinkAmount: {pointsSpent * unitsScaledPerScalePoint}");
            currentScalePoints -= pointsSpent;
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
        state = value.isPressed ? ScaleState.Growing : ScaleState.None;
    }

    public void OnShrinkObject(InputValue value)
    {
        if (playerMode.GetPlayerState() == PlayerState.Grab || state == ScaleState.Growing) return;
        state = value.isPressed ? ScaleState.Shrinking : ScaleState.None;
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
