using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VOTrigger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool played = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !played)
        {
            audioSource.Play();
            played = true;
        }
    }
}
