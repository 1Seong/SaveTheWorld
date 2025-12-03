using DG.Tweening;
using UnityEngine;

public class Curtain : ToggleInteractives
{
    public float fullScale = 1.395f;
    public float foldScale = .4f;

    protected override void On()
    {
        isActing = true;
        transform.DOScaleX(foldScale, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        transform.DOScaleX(fullScale, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
