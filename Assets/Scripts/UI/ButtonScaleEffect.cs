using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1.1f;
    public float duration = 0.5f;

    private Vector3 originScale;

    private void Awake()
    {
        originScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originScale;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originScale * scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originScale * scaleFactor, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originScale, duration);
    }

    private void OnDisable()
    {
        transform.localScale = originScale;
    }

    private void OnEnable()
    {
        transform.localScale = originScale;
    }
}
