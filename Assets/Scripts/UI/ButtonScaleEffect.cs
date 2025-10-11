using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class ButtonScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1.1f;
    public float duration = 0.5f;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * scaleFactor, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, duration);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }
}
