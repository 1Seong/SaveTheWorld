using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CreditPanel credits;

    public void OpenOption()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_MainButton);

        OptionPanel.Instance.Show();
    }

    public void OpenCredits()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_MainButton);

        credits.Show();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
