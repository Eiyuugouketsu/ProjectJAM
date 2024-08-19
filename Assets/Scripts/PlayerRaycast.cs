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
    ScalableObject currentTarget;
    ScalableObject currentGrabbableTarget;


    private void Update()
    {
        PerformRaycast();
        PerformGrabRaycast();
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
            }
            OnMouseOverScalableObject(null);
            currentTarget = null;
            return;
        }
        
        ScalableObject hitObject = hit.collider.gameObject.GetComponent<ScalableObject>();
        if (currentTarget != hitObject || forceUpdate)
        {
            OnMouseOverScalableObject?.Invoke(hitObject);
            if (currentTarget != null)
            {
                currentTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
            }
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
            if (currentGrabbableTarget != null)
            {
                currentGrabbableTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
            }
            OnMouseOverGrabbableObject(null);
            currentGrabbableTarget = null;
            return;
        }

        ScalableObject grabObject = grabHit.collider.gameObject.GetComponent<ScalableObject>();
        if (currentGrabbableTarget != grabObject)
        {
            OnMouseOverGrabbableObject?.Invoke(grabObject);
            if (currentGrabbableTarget != null)
            {
                currentGrabbableTarget.GetComponent<OutlineManager>().ToggleOutlineActive(false);
            }
            currentGrabbableTarget = grabObject;
            currentGrabbableTarget.GetComponent<OutlineManager>().ToggleOutlineActive(true);
        }
    }
}
