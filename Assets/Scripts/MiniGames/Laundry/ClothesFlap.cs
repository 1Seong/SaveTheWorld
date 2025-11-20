using DG.Tweening;
using UnityEngine;

public class ClothesFlap : MonoBehaviour
{
    public float angleScale = 10f;   // windX 1당 회전 각도
    public float maxAngle = 45f;     // 최대 회전 제한
    public float smoothTime = 0.15f; // 탄성 반응 속도 (0.1~0.25)
    public float swayAmp = 1f;
    public float swayFreq = 8f;


    float currentAngle;
    float angleVelocity;
    Quaternion baseRot;

    private void Awake()
    {
        baseRot = transform.localRotation;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        float windX = BalanceGame.instance.WindForce.x;

        // windX = -3 ~ +3 같은 값이 들어온다고 가정
        float target = Mathf.Clamp(windX * angleScale, -maxAngle, maxAngle);

        float originalAngle = currentAngle;

        // 부드러운 탄성 회전
        currentAngle = Mathf.SmoothDampAngle(
            currentAngle,
            target,
            ref angleVelocity,
            smoothTime);

        float sway = Mathf.Sin(Time.time * swayFreq) * swayAmp;

        transform.localRotation = baseRot * Quaternion.Euler(currentAngle + sway, 0, 0); 
    }
}
