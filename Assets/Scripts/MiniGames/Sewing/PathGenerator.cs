using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [Header("Path Settings")]
    public float startX = 0f;
    public float endX = 0f;
    public float startY = 0f;
    public float endY = 50f;
    public float width = 3f;

    public int nodeCount = 12;
    public float noiseScale = 0.2f;
    public float noiseAmplitude = 5f;

    [Header("Spline Settings")]
    public int splineResolution = 20;

    [Header("Line Renderers")]
    public LineRenderer leftLine;
    public LineRenderer rightLine;

    [SerializeField] private List<Vector2> controlPoints = new List<Vector2>();
    [SerializeField] private List<Vector2> splinePoints = new List<Vector2>();

    private void Awake()
    {
        GeneratePath();
    }

    public void GeneratePath()
    {
        GenerateControlPoints();
        GenerateSpline();
        GenerateBoundaryLines();
    }

    //------------------------------------------------------
    // 1. Control Points 생성
    //------------------------------------------------------
    private void GenerateControlPoints()
    {
        controlPoints.Clear();

        var initPoint = Mathf.PerlinNoise(0f, 0f);

        for (int i = 0; i < nodeCount; i++)
        {
            float t = (float)i / (nodeCount - 1);

            float y = Mathf.Lerp(startY, endY, t);
            float baseX = Mathf.Lerp(startX, endX, t);

            float noise = Mathf.PerlinNoise(0f, i * noiseScale);
            float offsetX = (noise - initPoint) * 2f * noiseAmplitude;

            float x = baseX + offsetX;

            controlPoints.Add(new Vector2(x, y));
        }
    }

    //------------------------------------------------------
    // 2. Catmull-Rom Spline 생성
    //------------------------------------------------------
    private void GenerateSpline()
    {
        splinePoints.Clear();

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Vector2 p0 = GetPoint(i - 1);
            Vector2 p1 = GetPoint(i);
            Vector2 p2 = GetPoint(i + 1);
            Vector2 p3 = GetPoint(i + 2);

            for (int s = 0; s < splineResolution; s++)
            {
                float t = (float)s / splineResolution;
                Vector2 pos = CatmullRom(p0, p1, p2, p3, t);
                splinePoints.Add(pos);
            }
        }
    }

    private Vector2 GetPoint(int i)
    {
        if (i < 0) return controlPoints[0];
        if (i >= controlPoints.Count) return controlPoints[controlPoints.Count - 1];
        return controlPoints[i];
    }

    private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    //------------------------------------------------------
    // 3. LineRenderer 2개로 경계 생성
    //------------------------------------------------------
    private void GenerateBoundaryLines()
    {
        if (leftLine == null || rightLine == null)
        {
            Debug.LogError("LineRenderers not assigned.");
            return;
        }

        int count = splinePoints.Count;

        leftLine.positionCount = count;
        rightLine.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            Vector2 center = splinePoints[i];

            Vector2 left = center + 0.5f * width * Vector2.left;
            Vector2 right = center + 0.5f * width * Vector2.right;

            leftLine.SetPosition(i, left);
            rightLine.SetPosition(i, right);
        }
    }

    //------------------------------------------------------
    // 옵션: 특정 y 에서의 중심 x 계산
    //------------------------------------------------------
    public float GetCenterXAtY(float y)
    {
        if (splinePoints.Count < 2)
            return 0f;

        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            if (splinePoints[i].y <= y && splinePoints[i + 1].y >= y)
            {
                float t = Mathf.InverseLerp(
                    splinePoints[i].y,
                    splinePoints[i + 1].y,
                    y
                );
                return Mathf.Lerp(splinePoints[i].x, splinePoints[i + 1].x, t);
            }
        }

        return splinePoints[splinePoints.Count - 1].x;
    }

    public bool IsInTheRoad(float x, float y)
    {
        var centerX = GetCenterXAtY(y);
        var leftX = centerX - 0.5f * width;
        var rightX = centerX + 0.5f * width;

        return x >= leftX && x <= rightX;
    }
}
