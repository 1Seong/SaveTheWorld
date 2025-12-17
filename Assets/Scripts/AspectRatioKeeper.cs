using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioKeeper : MonoBehaviour
{
    public Camera uiCamera;
    public float targetAspect = 16f / 9f;
    int lastW, lastH;


    void Start()
    {
        Apply();
    }

    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH)
            Apply();
    }

    void Apply()
    {
        lastW = Screen.width;
        lastH = Screen.height;

        float windowAspect = (float)Screen.width / Screen.height;
        float scale = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        Rect rect;

        if (scale < 1.0f)
        {
            // 위아래 여백
            rect = new Rect(
                0,
                (1 - scale) / 2,
                1,
                scale
            );
        }
        else
        {
            // 좌우 여백
            float invScale = 1 / scale;
            rect = new Rect(
                (1 - invScale) / 2,
                0,
                invScale,
                1
            );
        }

        cam.rect = rect;
        if(uiCamera != null)
            uiCamera.rect = rect;
    }
}
