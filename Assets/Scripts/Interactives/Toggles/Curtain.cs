using DG.Tweening;
using UnityEngine;

public class Curtain : ToggleInteractives
{
    protected override void On()
    {
        isActing = true;
        transform.DOScaleX(0.6f, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        transform.DOScaleX(3.6f, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
