using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomConvertButton : MonoBehaviour
{
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
        ItemManager.Instance.TurnOffGoButtons();
        BottomButton.SetActive(true);
        StageManager.Instance.ConvertViewCeiling();
    }

    public void ReturnSide()
    {
        if (GameManager.Instance.IsTurning) return;

        EventSystem.current.SetSelectedGameObject(null);
        ItemManager.Instance.TurnOnGoButtons();
        gameObject.SetActive(false);
        StageManager.Instance.ReturnToSide();
    }
}
