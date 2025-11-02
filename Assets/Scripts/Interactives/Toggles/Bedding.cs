using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Bedding : ToggleInteractives
{
    [SerializeField] private float targetMove = 1f;
    [SerializeField] private float duration = 0.4f;
    public UnityEvent OnEvents;
    public UnityEvent OffEvents;

    protected override void On()
    {
        isActing = true;
        OnEvents?.Invoke();
        transform.DOMoveY(transform.position.y + targetMove, duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        OffEvents?.Invoke();
        transform.DOMoveY(transform.position.y - targetMove, duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
