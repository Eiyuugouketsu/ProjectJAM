using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScale : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    [SerializeField] float minimumScale;
    [SerializeField] Rigidbody rb;
    float baseMass;
    float baseScale;
    float scaleValue = 0;
    List<Transform> touchingObjects = new List<Transform>();

    private void OnCollisionEnter(Collision other) 
    {
        touchingObjects.Add(other.transform);
    }

    private void OnCollisionExit(Collision other) 
    {
        touchingObjects.Remove(other.transform);
    }
    private void Start() 
    {
        baseMass = rb.mass;
        baseScale = transform.localScale.x;
    }

    public void Grow(float growAmount)
    {
        if (touchingObjects.Count <= 2) {
            scaleValue += growAmount * scaleSpeed; 
        }
    }
         
    public void Shrink(float shrinkAmount)
    {
        scaleValue -= shrinkAmount * scaleSpeed;
    }

     public void Stop()
    {
        scaleValue = 0;
    }
    private void FixedUpdate() 
    {
        if(transform.localScale.x + scaleValue < minimumScale) return;
        transform.localScale = new Vector3(transform.localScale.x + scaleValue,transform.localScale.x + scaleValue,transform.localScale.x + scaleValue);
        rb.mass = Mathf.Pow(transform.localScale.x/baseScale,3)*baseMass;
    }

    public float GetMass()
    {
        return rb.mass;
    }

    public float GetMinimumScale()
    {
        return minimumScale;
    }
}
