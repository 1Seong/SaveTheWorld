
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public Transform[] tiles;          // 배경 조각들 (왼→오 순서 추천)
    public float moveSpeed = 5f;       // 왼쪽 이동 속도
    public float tileWidth = 20f;      // 각각의 배경 조각 너비
    public float bias = 0.1f;

    private void Update()
    {
        foreach (var t in tiles)
        {
            t.localPosition += moveSpeed * Time.deltaTime * Vector3.left;

            // 왼쪽으로 충분히 벗어났으면 가장 오른쪽으로 이동
            float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

            if (t.localPosition.x + tileWidth + bias <= leftLimit)
            {
                // 현재 타일들 중 가장 오른쪽 위치 계산
                float maxX = GetMaxRightX();
                t.localPosition = new Vector3(maxX + tileWidth, t.localPosition.y, t.localPosition.z);
            }
        }
    }

    private float GetMaxRightX()
    {
        float maxX = float.MinValue;
        foreach (var t in tiles)
        {
            if (t.localPosition.x > maxX)
                maxX = t.localPosition.x;
        }
        return maxX;
    }
}
