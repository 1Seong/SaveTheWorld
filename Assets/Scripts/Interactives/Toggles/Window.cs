using DG.Tweening;
using UnityEngine;

public class Window : ToggleInteractives
{
    protected override void On()
    {
        transform.DOMoveX(transform.position.x - 2.5f, 0.6f).SetEase(Ease.InCirc);
    }

    protected override void Off()
    {
        transform.DOMoveX(transform.position.x + 2.5f, 0.6f).SetEase(Ease.InCirc);
    }
}
