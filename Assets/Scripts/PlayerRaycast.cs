using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public event Action<ScalableObject> OnMouseOverScalableObject;
    [SerializeField] Transform cameraRoot;
    [SerializeField] float checkForObjectDistance;
    [SerializeField] LayerMask layerMask;
    ScalableObject currentTarget;

    private void Update()
    {
        PerformRaycast();
    }

    public void PerformRaycast(bool forceUpdate = false)
    {
        Debug.DrawLine(cameraRoot.transform.position, transform.position + (cameraRoot.forward).normalized * checkForObjectDistance, Color.red);
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
}
