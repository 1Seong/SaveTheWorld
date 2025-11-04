using DG.Tweening;
using UnityEngine;

public class HoverShake : MonoBehaviour
{
    [SerializeField] private float rotAngle = 20f;
    [SerializeField] private float duration = 0.3f;
    private bool isActing = false;

    void OnMouseEnter()
    {
        if (isActing) return;
        isActing = true;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(transform.localRotation.eulerAngles + new Vector3(0f, rotAngle, 0f), duration).SetEase(Ease.OutCirc))
            .Append(transform.DOLocalRotate(transform.localRotation.eulerAngles - new Vector3(0f, rotAngle, 0f), duration).SetEase(Ease.OutCirc))
            .Append(transform.DOLocalRotate(transform.localRotation.eulerAngles, duration).SetEase(Ease.OutCirc).OnComplete(()=>
            {
                isActing = false;
            }));
        seq.Play();
    }
}
