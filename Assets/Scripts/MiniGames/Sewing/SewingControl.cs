using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SewingControl : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Others")]
    public Transform needle;
    public float downDis = 0.8f;
    public float moveDur = 0.3f;
    public TrailRenderer trailRenderer;
    public Material trailMaterial;
    public Transform leftHand;
    public Transform rightHand;
    public float handMoveOffset = 0.5f;
    public float handMoveSpeed = 1f;
    public PathGenerator pathGenerator;
    public Camera cam;

    [Header("Shaking")]
    public float[] shakeStartCooltimes;
    public float[] shakeDurations;
    public float shakeMagnitude = 0.3f;
    public UniversalRendererData urpData;
    public Material fullScreenShaderMat;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Tween needleTween;
    private Vector3 rightOriginPosition;
    private Vector3 leftOriginPosition;
    private bool rightReturning = false;
    private bool leftReturning = false;
    private bool isShaking = false;
    private Tween leftShakeTween;
    private Tween rightShakeTween;
    private int cnt = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        needleTween = needle.DOLocalMoveY(needle.localPosition.y + downDis, moveDur).SetLoops(-1, LoopType.Yoyo);
        needleTween.Pause();

        rightOriginPosition = rightHand.localPosition;
        leftOriginPosition = leftHand.localPosition;

        InvokeRepeating(nameof(ShakeHand), shakeStartCooltimes[0] - shakeDurations[0], shakeStartCooltimes[0]);
    }

    private void ShakeHand()
    {
        FullScreenPassRendererFeature rf;
        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = fullScreenShaderMat;
            rf.SetActive(true);
        }

        isShaking = true;
        leftShakeTween = leftHand.DOShakePosition(shakeDurations[cnt], 0.01f, 10);
        rightShakeTween = rightHand.DOShakePosition(shakeDurations[cnt], 0.01f, 10).OnComplete(() =>
        {
            FullScreenPassRendererFeature rf;
            if (urpData.TryGetRendererFeature(out rf))
            {
                rf.passMaterial = null;
                rf.SetActive(false);
            }

            isShaking = false;
        });
    }

    private void OnDestroy()
    {
        needleTween.Kill();
        trailRenderer.startColor = Color.red;
        trailRenderer.endColor = Color.red;
        trailMaterial.color = Color.red;

        FullScreenPassRendererFeature rf;
        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = null;
            rf.SetActive(false);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

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
            if (!rightReturning)
            {
                rightHand.localPosition = Vector3.Lerp(
                    rightHand.localPosition,
                    targetPos,
                    Time.deltaTime * handMoveSpeed
                );
                if ((rightHand.localPosition - rightOriginPosition).magnitude >= handMoveOffset * 0.8f)
                {
                    rightReturning = true;
                }
            }
            else
            {
                // 현재 거리
                float dist = (rightHand.localPosition - rightOriginPosition).magnitude;

                // 복귀 진행도: 1 → 0
                float progress = Mathf.Clamp01(dist / handMoveOffset);

                // Z값을 올렸다가 내려보내는 곡선
                // 최대 0.05f까지 올라간 후 다시 내려감
                float lift = Mathf.Sin(progress * Mathf.PI) * 0.008f;

                // 원점 방향 보간
                Vector3 nextPos = Vector3.Lerp(
                    rightHand.localPosition,
                    rightOriginPosition,
                    Time.deltaTime * handMoveSpeed
                );

                // 여기에 z곡선 추가
                nextPos.z = rightOriginPosition.z + lift;

                rightHand.localPosition = nextPos;
                if ((rightHand.localPosition - rightOriginPosition).magnitude <= handMoveOffset * 0.2f)
                {
                    rightReturning = false;
                }
            }
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
            if (!leftReturning)
            {
                leftHand.localPosition = Vector3.Lerp(
                    leftHand.localPosition,
                    targetPos,
                    Time.deltaTime * handMoveSpeed
                );
                if ((leftHand.localPosition - leftOriginPosition).magnitude >= handMoveOffset * 0.8f)
                {
                    leftReturning = true;
                }
            }
            else
            {
                // 현재 거리
                float dist = (leftHand.localPosition - leftOriginPosition).magnitude;

                // 복귀 진행도: 1 → 0
                float progress = Mathf.Clamp01(dist / handMoveOffset);

                // Z값을 올렸다가 내려보내는 곡선
                // 최대 0.05f까지 올라간 후 다시 내려감
                float lift = Mathf.Sin(progress * Mathf.PI) * 0.008f;

                // 원점 방향 보간
                Vector3 nextPos = Vector3.Lerp(
                    leftHand.localPosition,
                    leftOriginPosition,
                    Time.deltaTime * handMoveSpeed
                );

                // 여기에 z곡선 추가
                nextPos.z = leftOriginPosition.z + lift;

                leftHand.localPosition = nextPos;
                if ((leftHand.localPosition - leftOriginPosition).magnitude <= handMoveOffset * 0.2f)
                {
                    leftReturning = false;
                }
            }
        }

        // 3) 입력 결합
        Vector2 desiredDirection = arrowInput + wasdInput;

        if (isShaking)
            desiredDirection += Random.insideUnitCircle * shakeMagnitude;

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
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        rb.linearVelocity = currentVelocity;
        CheckGameEnd();
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

    private void CheckGameEnd()
    {
        if(transform.localPosition.y >= pathGenerator.endY)
        {
            MiniGameManager.instance.IsPlaying = false;

            ++cnt;

            FullScreenPassRendererFeature rf;
            if (urpData.TryGetRendererFeature(out rf))
            {
                rf.passMaterial = null;
                rf.SetActive(false);
            }

            CancelInvoke(nameof(ShakeHand));
            isShaking = false;
            leftShakeTween.Kill();
            rightShakeTween.Kill();

            trailRenderer.startColor = Color.green;
            trailRenderer.endColor = Color.green;
            trailMaterial.color = Color.green;
            rb.linearVelocity = Vector2.zero;
            needleTween.Pause();
            currentVelocity = Vector2.zero;
            rightHand.localPosition = rightOriginPosition;
            leftHand.localPosition = leftOriginPosition;

            if(cnt < MiniGameManager.instance.GameRepeatNum)
                InvokeRepeating(nameof(ShakeHand), 2.5f + shakeStartCooltimes[cnt] - shakeDurations[cnt], shakeStartCooltimes[cnt]);

            SewingGame.instance.GameEnd();
        }
        else if(!pathGenerator.IsInTheRoad(transform.localPosition.x, transform.localPosition.y))
        {
            MiniGameManager.instance.IsPlaying = false;

            ++cnt;

            FullScreenPassRendererFeature rf;
            if (urpData.TryGetRendererFeature(out rf))
            {
                rf.passMaterial = null;
                rf.SetActive(false);
            }

            CancelInvoke(nameof(ShakeHand));
            isShaking = false;
            leftShakeTween.Kill();
            rightShakeTween.Kill();

            trailRenderer.startColor = Color.black;
            trailRenderer.endColor = Color.black;
            trailMaterial.color = Color.black;
            rb.linearVelocity = Vector2.zero;
            needleTween.Pause();
            currentVelocity = Vector2.zero;
            rightHand.localPosition = rightOriginPosition;
            leftHand.localPosition = leftOriginPosition;

            if (cnt < MiniGameManager.instance.GameRepeatNum)
                InvokeRepeating(nameof(ShakeHand), 2.5f + shakeStartCooltimes[cnt] - shakeDurations[cnt], shakeStartCooltimes[cnt]);

            SewingGame.instance.GameEnd();
        }
    }
}
