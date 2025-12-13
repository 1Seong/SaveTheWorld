using UnityEngine;

public class MiniGameClearButton : MonoBehaviour
{
    public Item.Interactives typeId;

    private void Start()
    {
        if (!GameManager.Instance.IsMiniGameCleared(typeId.ToString()))
            gameObject.SetActive(false);
    }

    public void OnClick()
    {
        SceneTransition.Instance.UnloadScene();
    }
}
