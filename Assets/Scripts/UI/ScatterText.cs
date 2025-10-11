using UnityEngine;
using DG.Tweening;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ScatterText : MonoBehaviour
{
    public float radius = 100f;        // ���콺 ���� �ݰ�
    public float scatterAmount = 30f;  // ������� �Ÿ�
    public float smooth = 8f;          // �̵� �ӵ�
    public float returnDelay = 0.5f;   // �ǵ��ư��� ���� �ð� (��)

    TMP_Text text;
    TMP_TextInfo textInfo;
    Vector3[][] originalVertices;
    RectTransform canvasRect;
    Camera camForCanvas;

    float[] returnTimers; // ���ں� Ÿ�̸�
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

                // ���콺 �ݰ� �ȿ� ���� �� ��� ����
                Vector2 dir = (charScreen - mouseScreen).normalized;
                Vector2 targetScreen = charScreen + dir * scatterAmount * t;

                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, targetScreen, camForCanvas, out Vector3 targetWorld))
                {
                    Vector3 targetLocal = text.transform.InverseTransformPoint(targetWorld);
                    targetOffset = targetLocal - charMidLocal;
                }

                returnTimers[i] = returnDelay; // ���� Ÿ�̸� �ʱ�ȭ
                
            }
            else
            {
                // ���콺 �ݰ� �� �� Ÿ�̸� ī��Ʈ�ٿ�
                returnTimers[i] -= Time.deltaTime;
                if (returnTimers[i] > 0f)
                {
                    // ���� ���� ��� �� �� �״�� ����
                    targetOffset = verts[vi] - baseVerts[vi];
                }
                else
                {
                    
                    // ���� ��
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
