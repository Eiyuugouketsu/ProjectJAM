using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScale : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    [SerializeField] float minimumScale;
    [SerializeField] Rigidbody rb;
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

    void Update()
    {
        //if(Input.GetKey(KeyCode.Q) && touchingObjects.Count <=2)
        //{
        //    scaleValue += scaleSpeed * Time.deltaTime;
        //}
        //else if(Input.GetKey(KeyCode.E))
        //{
        //    scaleValue -= scaleSpeed * Time.deltaTime;
        //}
        //else
        //{
        //    scaleValue = 0;
        //}
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
