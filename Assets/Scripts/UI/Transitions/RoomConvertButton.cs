using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomConvertButton : MonoBehaviour
{
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
}
