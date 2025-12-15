
using UnityEngine;

public class PencilSoundControl : MonoBehaviour
{
    public void StartDraw()
    {
        AudioManager.Instance.LoopSfxOn(AudioType.SFX_P_Draw);
    }

    public void EndDraw()
    {
        AudioManager.Instance.LoopSfxOff();
    }

    public void PencilBreak()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_P_Break);
    }

}
