using UnityEngine;

public class MiniGameClearButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.MiniGameClear();
    }
}
