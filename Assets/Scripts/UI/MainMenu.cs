using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CreditPanel credits;

    public void StartGame()
    {
        SceneTransition.Instance.LoadScene("MainScene");
    }

    public void OpenOption()
    {
        OptionPanel.Instance.Show();
    }

    public void OpenCredits()
    {
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
