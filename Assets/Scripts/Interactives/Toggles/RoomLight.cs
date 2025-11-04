using DG.Tweening;
using UnityEngine;

public class RoomLight : ToggleInteractives
{
    [SerializeField] private GameObject localVolume;
    [SerializeField] private Transform lineEnd;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float targetY = -0.5f;

    protected override void On()
    {
        isActing = true;
        localVolume.SetActive(true);

        var seq = DOTween.Sequence(lineEnd);
        seq.Append(lineEnd.DOMoveY(lineEnd.transform.position.y + targetY, duration / 2f))
            .Append(lineEnd.DOMoveY(lineEnd.transform.position.y, duration / 2f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                isActing = false;
            }));
        seq.Play();
    }

    protected override void Off()
    {
        isActing = true;
        localVolume.SetActive(false);
        var seq = DOTween.Sequence(lineEnd);
        seq.Append(lineEnd.DOMoveY(lineEnd.transform.position.y+targetY, duration / 2f))
            .Append(lineEnd.DOMoveY(lineEnd.transform.position.y, duration / 2f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                isActing = false;
            }));
        seq.Play();
    }
}
