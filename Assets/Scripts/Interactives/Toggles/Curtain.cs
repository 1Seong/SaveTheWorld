using DG.Tweening;
using UnityEngine;

public class Curtain : ToggleInteractives
{
    public float fullScale;
    public float foldScale = .4f;

    private void Start()
    {
        fullScale = transform.localScale.x;
    }

    protected override void On()
    {
        isActing = true;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Curtain);

        transform.DOScaleX(foldScale, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Curtain);

        transform.DOScaleX(fullScale, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
