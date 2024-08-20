using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public event Action<ScalableObject> OnMouseOverScalableObject;
    public event Action<ScalableObject> OnMouseOverGrabbableObject;

    [SerializeField] Transform cameraRoot;
    [SerializeField] float checkForObjectDistance;
    [SerializeField] float checkForGrabObjectDistance;
    [SerializeField] LayerMask layerMask;
    public ScalableObject currentTarget;
    // ScalableObject currentGrabbableTarget;

    private void Update()
    {
        if (PlayerThresholds.Instance.PlayerMode.GetPlayerState() == PlayerState.Scale)
        {
            if (PlayerThresholds.Instance.PlayerScalePower.GetState() != ScaleState.Growing
            && PlayerThresholds.Instance.PlayerScalePower.GetState() != ScaleState.Shrinking) PerformRaycast();
        } else
        {
            PerformGrabRaycast();
        }
    }

    public void PerformRaycast(bool forceUpdate = false)
    {
        Debug.DrawLine(cameraRoot.transform.position, cameraRoot.transform.position + (cameraRoot.forward).normalized * checkForObjectDistance, Color.red);

        RaycastHit hit;
        Physics.Raycast(cameraRoot.transform.position, cameraRoot.forward, out hit, checkForObjectDistance, layerMask);

        if (!hit.collider || !hit.collider.gameObject)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
                OnMouseOverScalableObject?.Invoke(null);
            }
            currentTarget = null;
            return;
        }
        
        ScalableObject hitObject = hit.collider.gameObject.GetComponent<ScalableObject>();
        if (currentTarget != hitObject || forceUpdate)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
            }
            if (currentTarget != hitObject) OnMouseOverScalableObject?.Invoke(hitObject);
            currentTarget = hitObject;
            currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(true);
        }
    }

    public void PerformGrabRaycast(bool forceUpdate = false)
    {
        Debug.DrawLine(cameraRoot.transform.position, cameraRoot.transform.position + (cameraRoot.forward).normalized * checkForGrabObjectDistance, Color.blue);
        RaycastHit grabHit;
        Physics.Raycast(cameraRoot.transform.position, cameraRoot.forward, out grabHit, checkForGrabObjectDistance, layerMask);
        if (!grabHit.collider || !grabHit.collider.gameObject)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
                OnMouseOverGrabbableObject?.Invoke(null);
            }
            currentTarget = null;
            return;
        }

        ScalableObject grabObject = grabHit.collider.gameObject.GetComponent<ScalableObject>();
        if (currentTarget != grabObject)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
            }
            if (currentTarget != grabObject) OnMouseOverGrabbableObject?.Invoke(grabObject);
            currentTarget = grabObject;
            currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(true);
        }
    }
}
