using DG.Tweening;
using UnityEngine;

public class Drawer : ToggleInteractives
{
    [SerializeField] private float targetMove = -1f;
    [SerializeField] private float targetScale = 1.4f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Transform innerTransform;

    protected override void On()
    {
        isActing = true;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Drawer);

        innerTransform.DOScaleY(targetScale, duration).SetEase(Ease.InOutCubic);
        transform.DOMoveY(transform.position.y + targetMove, duration).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Drawer);

        innerTransform.DOScaleY(0f, duration).SetEase(Ease.InOutCubic);
        transform.DOMoveY(transform.position.y - targetMove, duration).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
