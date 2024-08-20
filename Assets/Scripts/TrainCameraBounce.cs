using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TrainCameraBounce : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource source;
    
    float timeToShake = 0f;

    void Update()
    {
        timeToShake -= Time.deltaTime;
        if (timeToShake <= 0f)
        {
            source.GenerateImpulse();
            timeToShake = Random.Range(1f, 7f);
        }
    }
}
