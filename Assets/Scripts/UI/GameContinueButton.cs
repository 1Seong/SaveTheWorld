using UnityEngine;
using UnityEngine.UI;

public class GameContinueButton : MonoBehaviour
{
    public bool isNewStart = false;

    private void Start()
    {
        if(!isNewStart && SaveManager.Instance.HasSave())
            GetComponent<Button>().interactable = true;
    }

    public void OnClick()
    {
        if(isNewStart)
        {
            SaveManager.Instance.DeleteSave();
            GameManager.Instance.ResetGameCleared();
        }

        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_MainButton);
        SceneTransition.Instance.LoadScene("MainScene");
    }
}
