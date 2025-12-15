using UnityEngine;

public class RCButton : MonoBehaviour
{
    [SerializeField] RCUI rcUI;
    [SerializeField] Transform pixelButton;
    [SerializeField] string num;

    private void OnMouseDown()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_RemoteButton);

        pixelButton.Translate(new Vector3(0f, -0.1f, 0f), Space.Self);

        if(num == "-1")
        {
            rcUI.DisableUI();
            pixelButton.Translate(new Vector3(0f, 0.1f, 0f), Space.Self);
            return;
        }

        if (rcUI.matched || rcUI.isActing) return;

        rcUI.Nums += num;
    }

    private void OnMouseUp()
    {
        pixelButton.Translate(new Vector3(0f, 0.1f, 0f), Space.Self);
    }

}
