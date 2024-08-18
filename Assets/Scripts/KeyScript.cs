using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class KeyScript : ScalableObject
{
    [Tooltip("A string for the key id. It only matches a keyhole that has the same key id.")]
    [SerializeField] string keyId;

    public string GetKeyId()
    {
        return keyId;
    }

    public void ConsumeKey()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        SetIsInteractable(false);
    }
}
