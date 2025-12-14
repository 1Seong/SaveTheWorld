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

        SceneTransition.Instance.LoadScene("MainScene");
    }
}
