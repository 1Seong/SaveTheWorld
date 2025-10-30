using DG.Tweening;
using UnityEngine;

public class Curtain : ToggleInteractives
{
    protected override void On()
    {
        transform.DOScaleX(0.6f, 0.6f).SetEase(Ease.OutBack);
    }

    protected override void Off()
    {
        transform.DOScaleX(3.6f, 0.6f).SetEase(Ease.OutBack);
    }
}
