using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CreditPanel credits;

    public void StartSavedGame()
    {
        SceneTransition.Instance.LoadScene("MainScene");
    }

    public void StartNewGame()
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
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
