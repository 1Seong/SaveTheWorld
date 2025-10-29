using System;
using UnityEngine;

public class ToggleInteractives : MonoBehaviour // template method pattern
{
    protected Action toggleOnAction;
    protected Action toggleOffAction;

    private bool _isActive = false;
    public bool IsActive 
    { 
        get { return _isActive; }

        set
        {
            _isActive = value;
            if(value) // true, on
            {
                toggleOnAction?.Invoke();
            }
            else // false, off
            {
                toggleOffAction?.Invoke();
            }
        }
    }

    public void OnClick()
    {
        IsActive = !IsActive;
    }
}
