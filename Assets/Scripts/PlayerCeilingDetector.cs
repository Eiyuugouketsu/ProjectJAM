using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilingDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ceiling")) PlayerThresholds.Instance.isCeilingAbove = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ceiling")) PlayerThresholds.Instance.isCeilingAbove = false;
    }
}
