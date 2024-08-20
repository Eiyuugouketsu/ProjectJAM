using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillation : MonoBehaviour
{
    
    public static float farthestDistance = 20;
    public static float speed = 0.003f;
    private float yOrigin;
    private float notTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        notTime = Random.Range(-1f, 1f);
        yOrigin = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = (Vector3.up * (yOrigin + Mathf.Sin(notTime) * farthestDistance)) + (transform.localPosition.x * Vector3.right);
        notTime += speed;
        //if(notTime >= float.MaxValue - 1)
        //{
        //    notTime = 0;
        //}
    }
}
