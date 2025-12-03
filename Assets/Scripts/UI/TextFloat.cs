using DG.Tweening;
using UnityEngine;

public class TextFloat : MonoBehaviour
{

    private Tween t;

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        t = rt.DOAnchorPosY(rt.anchoredPosition.y + 11f, 2.7f)
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        t.Kill();
    }
}
