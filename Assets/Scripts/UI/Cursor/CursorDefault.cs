using UnityEngine;

public class CursorDefault : MonoBehaviour
{
    [SerializeField] Sprite defaultCursorSprite;

    void Awake()
    {
        Cursor.SetCursor(CursorUtility.ExtractTexture(defaultCursorSprite), Vector2.zero, CursorMode.Auto);
    }
}
