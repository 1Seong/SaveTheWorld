using UnityEngine;

public class ViewCursorFollow : MonoBehaviour
{
    public static float angleApplyRate = 1f;

    public float maxAngle = 10f;
    public float sensitivity = 0.1f;
    public float smooth = 5f;

    private Quaternion originRot;

    void Start()
    {
        originRot = transform.localRotation;
    }

    void Update()
    {
        if(!GameManager.Instance.IsPlaying) return;
        if(GameManager.Instance.IsTurning) return;

        float mx = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float my = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;
        
        float tiltX = Mathf.Clamp(-my * maxAngle * angleApplyRate, -maxAngle * angleApplyRate, maxAngle * angleApplyRate);
        float tiltY = Mathf.Clamp(mx * maxAngle * angleApplyRate, -maxAngle * angleApplyRate, maxAngle * angleApplyRate);
        
        Quaternion targetRot = originRot * Quaternion.Euler(tiltX, tiltY, 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smooth);
    }

    public void ChangeOriginRot(Quaternion targetRot)
    {
        originRot = targetRot;
    }
}
