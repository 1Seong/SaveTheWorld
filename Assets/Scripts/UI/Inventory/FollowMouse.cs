using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    RectTransform rectTransform;
    Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            Camera.main,
            out pos
        );

        rectTransform.anchoredPosition = pos;
    }
}
