using UnityEngine;

public class FlyClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Fly);

        GetComponent<Animator>().SetTrigger("Click");
    }
}
