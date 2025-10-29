using DG.Tweening;
using UnityEngine;

public class Curtain : ToggleInteractives
{
    private void Awake()
    {
        toggleOnAction += Open;
        toggleOffAction += Close;
    }

    private void Open()
    {
        transform.DOScaleX(0.6f, 0.6f).SetEase(Ease.OutBack);
    }

    private void Close()
    {
        transform.DOScaleX(3.6f, 0.6f).SetEase(Ease.OutBack);
    }
}
