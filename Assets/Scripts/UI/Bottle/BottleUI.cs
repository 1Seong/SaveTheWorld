using DG.Tweening;
using UnityEngine;

public class BottleUI : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private Transform pixelBodyT;
    [SerializeField] private GameObject bottleCanvas;

    public float maxRotation = 180f;     // 회전 제한 각도
    public float maxLift = 2f;           // 최대 상승 높이
    private float currentY;              // 초기 y위치
    private float currentRotation = 0f;  // 누적 회전값

    private bool _isComplete = false;
    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            _isComplete = value;
            if (value)
            {
                flowOut();
            }
        }
    }


    void Start()
    {
        currentY = transform.position.y;
    }

    void OnMouseDrag()
    {
        if (IsComplete) return;

        float mouseX = Input.GetAxis("Mouse X");
        float delta = -mouseX * rotationSpeed * Time.deltaTime;

        // 새 회전값 계산 및 제한
        float newRotation = Mathf.Clamp(currentRotation + delta, -maxRotation, maxRotation);
        float appliedDelta = newRotation - currentRotation;

        // 회전 적용
        transform.Rotate(Vector3.forward, appliedDelta, Space.Self);
        pixelBodyT.Rotate(Vector3.forward, appliedDelta, Space.Self);
        currentRotation = newRotation;

        // 회전 정도에 따른 y위치 보간
        float t = Mathf.InverseLerp(0, maxRotation, Mathf.Abs(currentRotation)); // 0~1
        float targetY = Mathf.Lerp(currentY, currentY + maxLift, t);

        Vector3 pos = transform.localPosition;
        pos.y = targetY;
        transform.localPosition = pos;
        pixelBodyT.localPosition = pos;

        if (Mathf.Abs(currentRotation) == maxRotation)
            IsComplete = true;
    }

    private void flowOut()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        Invoke(nameof(giveItem), 0.7f);
    }

    private void giveItem()
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        var item = GetComponentInChildren<ItemObject>(true);
        item.gameObject.SetActive(true);
        item.transform.DOMoveY(item.transform.position.y - 1f, 0.5f).SetEase(Ease.OutCirc);
    }

    public void EnableUI()
    {
        bottleCanvas.SetActive(true);
        ItemManager.Instance.TurnOffGoButtons();
        NoteManager.Instance.gameObject.SetActive(false);
    }

    public void DisableUI()
    {
        bottleCanvas.SetActive(false);
        ItemManager.Instance.TurnOnGoButtons();
        NoteManager.Instance.gameObject.SetActive(true);
    }
}
