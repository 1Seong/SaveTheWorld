using UnityEngine;
using UnityEngine.UI;

public class GameContinueButton : MonoBehaviour
{

    private void Start()
    {
        if(SaveManager.Instance.HasSave())
            GetComponent<Button>().interactable = true;
    }
}
