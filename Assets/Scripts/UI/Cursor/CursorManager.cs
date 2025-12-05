using UnityEngine;

public enum CursorType { Interact, Collect, CloseUp, CloseOut, Left, Right, Up, Down}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public Sprite interactSprite;
    public Sprite normalSprite;
    public Sprite collectSprite;
    public Sprite closeUpSprite;
    public Sprite closeOutSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite downSprite;

    Texture2D normalTex;

    Texture2D interactTex;
    Texture2D collectTex;
    Texture2D closeUpTex;
    Texture2D closeOutTex;
    Texture2D leftTex;
    Texture2D rightTex;
    Texture2D upTex;
    Texture2D downTex;

    int hoverCount = 0;

    void Awake()
    {
        Instance = this;
        normalTex = CursorUtility.ExtractTexture(normalSprite);

        interactTex = CursorUtility.ExtractTexture(interactSprite);
        closeUpTex = CursorUtility.ExtractTexture(closeUpSprite);
        closeOutTex = CursorUtility.ExtractTexture(closeOutSprite);
        leftTex = CursorUtility.ExtractTexture(leftSprite);
        rightTex = CursorUtility.ExtractTexture(rightSprite);
        upTex = CursorUtility.ExtractTexture(upSprite);
        downTex = CursorUtility.ExtractTexture(downSprite);
        collectTex = CursorUtility.ExtractTexture(collectSprite);
    }

    public void RequestHover(CursorType t)
    {
        hoverCount++;
        Texture2D targetTex = null;

        switch(t)
        {
            case CursorType.Interact:
                targetTex = interactTex; 
                break;
            case CursorType.Left:
                targetTex = leftTex;
                break;
            case CursorType.Right:
                targetTex = rightTex;
                break;
            case CursorType.Up:
                targetTex = upTex;
                break;
            case CursorType.Down:
                targetTex = downTex;
                break;
            case CursorType.Collect:
                targetTex = collectTex;
                break;
            case CursorType.CloseOut:
                targetTex = closeOutTex;
                break;
            case CursorType.CloseUp:
                targetTex = closeUpTex;
                break;
        }
        Cursor.SetCursor(targetTex, Vector2.zero, CursorMode.Auto);
    }

    public void ReleaseHover()
    {
        hoverCount = Mathf.Max(0, hoverCount - 1);
        if (hoverCount == 0)
            Cursor.SetCursor(normalTex, Vector2.zero, CursorMode.Auto);
    }
}
