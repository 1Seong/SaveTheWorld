using DG.Tweening;
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

    public virtual void OnClick()
    {
        IsActive = !IsActive;
    }

    protected virtual void Awake()
    {
        toggleOnAction += On;
        toggleOffAction += Off;
    }

    protected virtual void On()
    {
    }

    protected virtual void Off()
    {
    }
}
