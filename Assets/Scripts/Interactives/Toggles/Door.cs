using DG.Tweening;
using UnityEngine;

public class Door : ToggleInteractives
{
    [SerializeField] private float targetRot = 0f;
    [SerializeField] private float duration = 0.4f;

    protected override void On()
    {
        isActing = true;
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, targetRot, 0f), duration).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, -targetRot, 0f), duration).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
