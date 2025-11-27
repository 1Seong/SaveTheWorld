using UnityEngine;

public class ViewCursorFollow : MonoBehaviour
{
    public float maxAngle = 10f;
    public float sensitivity = 0.1f;
    public float smooth = 5f;

    private Quaternion originRot;

    void Start()
    {
        OptionPanel.MouseSensitivityOnValueChangedEvent += SetMaxAngle;
        originRot = transform.localRotation;
    }

    private void OnDestroy()
    {
        OptionPanel.MouseSensitivityOnValueChangedEvent -= SetMaxAngle;
    }

    private void SetMaxAngle(float value)
    {
        maxAngle = value;
    }

    void Update()
    {
        if(!GameManager.Instance.IsPlaying) return;
        if(GameManager.Instance.IsTurning) return;

        float mx = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float my = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;
        
        float tiltX = Mathf.Clamp(-my * maxAngle, -maxAngle, maxAngle);
        float tiltY = Mathf.Clamp(mx * maxAngle, -maxAngle, maxAngle);
        
        Quaternion targetRot = originRot * Quaternion.Euler(tiltX, tiltY, 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smooth);
    }

    public void ChangeOriginRot(Quaternion targetRot)
    {
        originRot = targetRot;
    }
}
