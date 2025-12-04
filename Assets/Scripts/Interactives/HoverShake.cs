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
        seq.Append(transform.DOLocalRotate(new Vector3(rotAngle, 0f, 0f), duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc))
            .Append(transform.DOLocalRotate(new Vector3(-2 * rotAngle, 0f, 0f), duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc))
            .Append(transform.DOLocalRotate(new Vector3(rotAngle, 0f, 0f), duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc).OnComplete(()=>
            {
                isActing = false;
            }));
        seq.Play();
    }
}
