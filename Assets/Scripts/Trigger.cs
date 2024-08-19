using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action<Collider> OnEventTriggerEnter;
    public event Action<Collider> OnEventTriggerExit;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnEventTriggerEnter?.Invoke(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnEventTriggerExit?.Invoke(other);
        }
    }
}
