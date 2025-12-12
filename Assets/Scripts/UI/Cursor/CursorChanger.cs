using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CursorType CursorType;

    public void SetHoverCursor()
    {
        CursorManager.Instance.RequestHover(CursorType);
    }

    public void SetNormalCursor()
    {
        CursorManager.Instance.ReleaseHover();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(TryGetComponent<Button>(out var button))
        {
            if (!button.interactable)
                return;
        }

        SetHoverCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetNormalCursor();
    }

    private void OnDisable()
    {
        // 오브젝트가 사라질 때 수동으로 복구
        SetNormalCursor();
    }
}
