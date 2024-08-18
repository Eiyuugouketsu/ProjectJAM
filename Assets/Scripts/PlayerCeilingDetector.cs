using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilingDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        PlayerThresholds.Instance.isCeilingAbove = true;
    }
    private void OnTriggerExit(Collider other) 
    {
        PlayerThresholds.Instance.isCeilingAbove = false;
    }
}
