using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaHitImage : MonoBehaviour, ICanvasRaycastFilter
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f;

    private Image image;
    private Sprite sprite;
    private Texture2D texture;

    void Awake()
    {
        image = GetComponent<Image>();
        sprite = image.sprite;

        if (sprite != null)
            texture = sprite.texture;
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (texture == null || sprite == null)
            return true;

        // 스크린 → 로컬 좌표
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform, screenPoint, eventCamera, out Vector2 local);

        // Rect 안에 없으면 무시
        Rect rect = image.GetPixelAdjustedRect();
        if (!rect.Contains(local))
            return false;

        // 0~1 정규화
        float x = (local.x - rect.x) / rect.width;
        float y = (local.y - rect.y) / rect.height;

        // Sprite 내 텍스처 좌표
        Rect texRect = sprite.textureRect;
        int texX = Mathf.FloorToInt(texRect.x + texRect.width * x);
        int texY = Mathf.FloorToInt(texRect.y + texRect.height * y);

        // 알파 검사
        Color c = texture.GetPixel(texX, texY);
        return c.a >= alphaThreshold;
    }
}

