//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    private Outline currentTarget;

    private PlayerRaycast playerRaycast;
    private PlayerGrabAbility playerGrabAbility;
    private PlayerScalePower playerScalePower;

    public Color selectionColor;
    public Color heldColor;
    public Color growColor;
    public Color shrinkColor;

    void OnEnable()
    {
        playerScalePower = GetComponent<PlayerScalePower>();

        if (TryGetComponent(out playerRaycast))
        {
            playerRaycast.OnMouseOverScalableObject += HandleOutline;
            
        }

        if (TryGetComponent(out playerGrabAbility))
        {
            playerGrabAbility.OnObjectPickedUp += HandleOutline;
            playerGrabAbility.OnObjectDropped  += AdjustHeldColor;
        }

        if (TryGetComponent(out playerScalePower))
        {
            playerScalePower.OnUpdateScalePoints += AdjustScaleColor;
        }
    }

    private void OnDisable()
    {
        playerRaycast.OnMouseOverScalableObject -= HandleOutline;

        if (playerGrabAbility != null)
        {
            playerGrabAbility.OnObjectPickedUp -= HandleOutline;
            playerGrabAbility.OnObjectDropped  -= AdjustHeldColor;
        }

        if (playerScalePower != null)
        {
            playerScalePower.OnUpdateScalePoints -= AdjustScaleColor;
        }

        currentTarget = null;
    } 

    void SetOutline(Component scalableObject)
    {
        if (scalableObject == null) return;
        Debug.Log("scalableObject is not null.");
        if (scalableObject.gameObject.TryGetComponent(out currentTarget))
        {
            currentTarget.enabled = false;
        }
    }

    void EnableOutlineOrAddIfAbsent(Component scalableObject)
    {
        if (scalableObject == null)
        {
            return;
        }
        if (scalableObject.gameObject.TryGetComponent(out currentTarget))
        {
            currentTarget.enabled = true;
            return;
        }

        currentTarget              = scalableObject.gameObject.AddComponent<Outline>();
        currentTarget.enabled      = true;
        currentTarget.OutlineColor = selectionColor;
    }

    public void HandleOutline(ScalableObject scalableObject)
    {
        SetOutline(scalableObject);
        EnableOutlineOrAddIfAbsent(scalableObject);
    }

    void AdjustHeldColor()
    {
        if (currentTarget == null) return;
        
        if (playerGrabAbility.GetIsHoldingObject())
        {
            currentTarget.OutlineColor = heldColor;
        }
    }

    void AdjustScaleColor(float dummy)
    {
        if (currentTarget == null)
        {
            return;
        }
        
        if (Input.GetMouseButton(0)) // Grow
        {
            currentTarget.OutlineColor = growColor;
        }
        else if (Input.GetMouseButton(1))
        {
            currentTarget.OutlineColor = shrinkColor;
        }
        else
        {
            currentTarget.OutlineColor = selectionColor;
        }
    }
}