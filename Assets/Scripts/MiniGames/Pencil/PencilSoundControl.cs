
using DG.Tweening;
using UnityEngine;

public class PencilSoundControl : MonoBehaviour
{
    public PencilGame pencilGame;

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

        transform.DOShakePosition(0.2f, 0.2f, 12);
    }

    public void GameStart()
    {
        pencilGame.GameStart();
    }

    public void StartCrying()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 0.1f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }
}
