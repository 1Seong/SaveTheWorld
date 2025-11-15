using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SewingControl : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Others")]
    public Transform needle;
    public float downDis = 0.8f;
    public float moveDur = 0.3f;
    public Transform leftHand;
    public Transform rightHand;
    public float handMoveOffset = 0.5f;
    public float handMoveSpeed = 1f;


    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Tween needleTween;
    private Vector3 rightOriginPosition;
    private Vector3 leftOriginPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        needleTween = needle.DOLocalMoveY(needle.localPosition.y + downDis, moveDur).SetLoops(-1, LoopType.Yoyo);
        rightOriginPosition = rightHand.localPosition;
        leftOriginPosition = leftHand.localPosition;
    }

    private void OnDestroy()
    {
        needleTween.Kill();
    }

    void Update()
    {
        // 1) 방향키 입력
        Vector2 arrowInput = new Vector2(
            GetKeyAxisHor(KeyCode.LeftArrow, KeyCode.RightArrow),
            GetKeyAxisVert(KeyCode.DownArrow)
        ).normalized;

        if(arrowInput.magnitude > 0)
        {
            // 목표 offset 계산
            Vector3 targetPos = rightOriginPosition + (Vector3)arrowInput * handMoveOffset;

            // 보간
            rightHand.localPosition = Vector3.Lerp(
                rightHand.localPosition,
                targetPos,
                Time.deltaTime * handMoveSpeed
            );
        }
        else
        {
            // 원래 위치로 천천히 복귀
            rightHand.localPosition = Vector3.Lerp(
                rightHand.localPosition,
                rightOriginPosition,
                Time.deltaTime * handMoveSpeed
            );
        }

        // 2) WASD 입력 (Unity Input Manager 기본 설정 기준)
        Vector2 wasdInput = new Vector2(
            GetKeyAxisHor(KeyCode.A, KeyCode.D),
            GetKeyAxisVert(KeyCode.S)
        ).normalized;

        if (wasdInput.magnitude > 0)
        {
            // 목표 offset 계산
            Vector3 targetPos = leftOriginPosition + (Vector3)wasdInput * handMoveOffset;

            // 보간
            leftHand.localPosition = Vector3.Lerp(
                leftHand.localPosition,
                targetPos,
                Time.deltaTime * handMoveSpeed
            );
        }
        else
        {
            // 원래 위치로 천천히 복귀
            leftHand.localPosition = Vector3.Lerp(
                leftHand.localPosition,
                leftOriginPosition,
                Time.deltaTime * handMoveSpeed
            );
        }

        // 3) 입력 결합
        Vector2 desiredDirection = arrowInput + wasdInput;

        // 목표 속도
        Vector2 targetVelocity = desiredDirection * maxSpeed;
        if (targetVelocity.magnitude > 0)
            needleTween.Play();
        else
            needleTween.Pause();

        // 4) 관성 처리
        if (desiredDirection.sqrMagnitude > 0.01f)
        {
            // 가속
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // 감속
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = currentVelocity;
    }

    // WASD 를 axis 로 변환해주는 보조 함수
    private float GetKeyAxisHor(KeyCode positive, KeyCode negative)
    {
        float v = 0f;
        if (Input.GetKey(positive)) v += 1f;
        if (Input.GetKey(negative)) v -= 1f;
        return v;
    }

    private float GetKeyAxisVert(KeyCode positive)
    {
        float v = 0f;
        if (Input.GetKey(positive)) v += 1f;
        return v;
    }
}
