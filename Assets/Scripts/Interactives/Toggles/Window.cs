using DG.Tweening;
using UnityEngine;

public class Window : ToggleInteractives
{
    private void Awake()
    {
        toggleOnAction += Open;
        toggleOffAction += Close;
    }

    private void Open()
    {
        transform.DOMoveX(transform.position.x - 2.5f, 0.6f).SetEase(Ease.InCirc);
    }

    private void Close()
    {
        transform.DOMoveX(transform.position.x + 2.5f, 0.6f).SetEase(Ease.InCirc);
    }
}
