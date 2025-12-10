using UnityEngine;

public class UIHoverFloat : MonoBehaviour
{
    public RectTransform target;      // 움직일 UI 이미지
    public float maxOffset = 20f;     // 최대 이동량 (px)
    public float followSpeed = 10f;   // 마우스 따라가는 속도
    public float returnSpeed = 5f;    // 원위치 복귀 속도
    public float hoverRadius = 100f;  // 마우스를 감지할 범위 (px)

    private Vector3 originalPos;
    private bool hovering = false;

    void Start()
    {
        if (target == null) target = GetComponent<RectTransform>();
        originalPos = target.localPosition;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        Vector2 mouseScreen = Input.mousePosition;

        // RectTransform 기준으로 마우스가 UI 위에 있는지 확인
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            target.parent as RectTransform,
            mouseScreen,
            null,
            out Vector2 localMouse);

        Vector2 localCenter = target.localPosition;

        // 마우스와 UI 중심과의 거리 계산
        float dist = Vector2.Distance(localMouse, localCenter);

        hovering = dist <= hoverRadius;

        if (hovering)
        {
            // 이동 방향 (local space)
            Vector2 dir = -(localMouse - localCenter).normalized;

            // 목표 offset 계산
            Vector3 targetPos = originalPos + (Vector3)(dir * maxOffset);

            // 보간
            target.localPosition = Vector3.Lerp(
                target.localPosition,
                targetPos,
                Time.deltaTime * followSpeed
            );
        }
        else
        {
            // 원래 위치로 천천히 복귀
            target.localPosition = Vector3.Lerp(
                target.localPosition,
                originalPos,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
