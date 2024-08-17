using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public event Action<TestScale> OnMouseOverScalableObject;
    [SerializeField] Transform cameraRoot;
    [SerializeField] float checkForObjectDistance;
    [SerializeField] LayerMask layerMask;

    private void Update()
    {
        Debug.DrawLine(cameraRoot.transform.position, transform.position + (cameraRoot.forward).normalized * checkForObjectDistance, Color.red);
        RaycastHit hit;
        Physics.Raycast(cameraRoot.transform.position, cameraRoot.forward, out hit, checkForObjectDistance, layerMask);
        if (!hit.collider || !hit.collider.gameObject)
        {
            OnMouseOverScalableObject(null);
            return;
        }
        
        TestScale hitObject = hit.collider.gameObject.GetComponent<TestScale>();
        // If affecting performance, cache the last value and only fire the event when getting a new value;
        OnMouseOverScalableObject?.Invoke(hitObject);
    }
}
