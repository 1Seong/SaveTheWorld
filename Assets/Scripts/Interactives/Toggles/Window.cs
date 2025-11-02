using DG.Tweening;
using UnityEngine;

public class Window : ToggleInteractives
{
    protected override void On()
    {
        isActing = true;
        transform.DOMoveX(transform.position.x - 2.5f, 0.6f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        transform.DOMoveX(transform.position.x + 2.5f, 0.6f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
