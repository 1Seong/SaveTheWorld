using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomConvertButton : MonoBehaviour
{
    [SerializeField] private GameObject RightButton;
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject TopButton;
    [SerializeField] private GameObject BottomButton;

    public void GoLeft()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StageManager.Instance.ConvertViewLeft();
    }

    public void GoRight()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StageManager.Instance.ConvertViewRight();
    }

    public void GoCeiling()
    {
        if (GameManager.Instance.IsTurning) return;

        EventSystem.current.SetSelectedGameObject(null);
        RightButton.SetActive(false);
        LeftButton.SetActive(false);
        BottomButton.SetActive(true);
        gameObject.SetActive(false);
        StageManager.Instance.ConvertViewCeiling();
    }

    public void ReturnSide()
    {
        if (GameManager.Instance.IsTurning) return;

        EventSystem.current.SetSelectedGameObject(null);
        RightButton.SetActive(true);
        LeftButton.SetActive(true);
        TopButton.SetActive(true);
        gameObject.SetActive(false);
        StageManager.Instance.ReturnToSide();
    }
}
