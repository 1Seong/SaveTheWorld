using UnityEngine;

[ExecuteAlways]
public class StaticCord : MonoBehaviour
{
    public Transform startPoint; // 천장 고정점
    public Transform endPoint;   // 전등 위치
    [Range(0f, 1f)] public float sagAmount = 0.5f; // 아래로 늘어지는 정도
    public int segmentCount = 20;
    public Vector3 sagDir = Vector3.down;

    private LineRenderer line;

    void OnValidate() => DrawCord();

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        DrawCord();
    }

    private void FixedUpdate()
    {
        DrawCord();
    }

    void DrawCord()
    {
        if (!line || !startPoint || !endPoint) return;

        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;
        Vector3 mid = (start + end) / 2f + sagDir * sagAmount;

        line.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            // Bezier curve (start → mid → end)
            Vector3 point = Mathf.Pow(1 - t, 2) * start +
                            2 * (1 - t) * t * mid +
                            Mathf.Pow(t, 2) * end;
            line.SetPosition(i, point);
        }
    }
}
