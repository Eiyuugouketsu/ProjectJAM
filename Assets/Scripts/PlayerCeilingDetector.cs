using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilingDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Ceiling")) PlayerThresholds.Instance.isCeilingAbove = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Ceiling")) PlayerThresholds.Instance.isCeilingAbove = false;
    }
}
