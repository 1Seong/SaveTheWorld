using UnityEngine;
using DG.Tweening;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ScatterText : MonoBehaviour
{
    public float radius = 100f;        // 마우스 영향 반경
    public float scatterAmount = 30f;  // 흩어지는 거리
    public float smooth = 8f;          // 이동 속도
    public float returnDelay = 0.5f;   // 되돌아가기 지연 시간 (초)

    TMP_Text text;
    TMP_TextInfo textInfo;
    Vector3[][] originalVertices;
    RectTransform canvasRect;
    Camera camForCanvas;

    float[] returnTimers; // 문자별 타이머
    bool[] scattered;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        if (text.canvas != null)
        {
            canvasRect = text.canvas.transform as RectTransform;
            camForCanvas = text.canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : (text.canvas.worldCamera ?? Camera.main);
        }
    }

    void Start()
    {
        CacheOriginalVertices();
    }

    void CacheOriginalVertices()
    {
        text.ForceMeshUpdate();
        textInfo = text.textInfo;
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        scattered = new bool[textInfo.characterCount];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var verts = textInfo.meshInfo[i].vertices;
            originalVertices[i] = new Vector3[verts.Length];
            System.Array.Copy(verts, originalVertices[i], verts.Length);

            scattered[i] = false;
        }
        returnTimers = new float[textInfo.characterCount];
    }

    void LateUpdate()
    {
        //text.ForceMeshUpdate();
        //textInfo = text.textInfo;

        Vector2 mouseScreen = Input.mousePosition;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vi = charInfo.vertexIndex;
            int mi = charInfo.materialReferenceIndex;

            Vector3[] verts = textInfo.meshInfo[mi].vertices;
            Vector3[] baseVerts = originalVertices[mi];

            Vector3 charMidLocal = (baseVerts[vi] + baseVerts[vi + 2]) * 0.5f;
            Vector3 charMidWorld = text.transform.TransformPoint(charMidLocal);
            Vector2 charScreen = RectTransformUtility.WorldToScreenPoint(camForCanvas, charMidWorld);

            float dist = Vector2.Distance(mouseScreen, charScreen);
            float t = Mathf.Clamp01(1f - (dist / radius));

            Vector3 targetOffset = Vector3.zero;

            if (t > 0f)
            {
                
                scattered[i] = true;

                // 마우스 반경 안에 있음 → 즉시 반응
                Vector2 dir = (charScreen - mouseScreen).normalized;
                Vector2 targetScreen = charScreen + dir * scatterAmount * t;

                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, targetScreen, camForCanvas, out Vector3 targetWorld))
                {
                    Vector3 targetLocal = text.transform.InverseTransformPoint(targetWorld);
                    targetOffset = targetLocal - charMidLocal;
                }

                returnTimers[i] = returnDelay; // 복귀 타이머 초기화
                
            }
            else
            {
                // 마우스 반경 밖 → 타이머 카운트다운
                returnTimers[i] -= Time.deltaTime;
                if (returnTimers[i] > 0f)
                {
                    // 아직 복귀 대기 중 → 그대로 유지
                    targetOffset = verts[vi] - baseVerts[vi];
                }
                else
                {
                    
                    // 복귀 중
                    targetOffset = Vector3.zero;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                int idx = vi + j;
                verts[idx] = Vector3.Lerp(verts[idx], baseVerts[idx] + targetOffset, Time.deltaTime * smooth);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
