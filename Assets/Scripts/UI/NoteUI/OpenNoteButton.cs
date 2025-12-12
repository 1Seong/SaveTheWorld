using UnityEngine;

public class OpenNoteButton : MonoBehaviour
{
    [SerializeField] private BackgroundAlphaControl background;
    [SerializeField] private GameObject UIBackground;

    public void OnClick()
    {
        if (NoteManager.Instance.isActive) // close
        {
            background.Hide();
            UIBackground.SetActive(false);
            NoteManager.Instance.Hide();
        }
        else // open
        {
            background.Show();
            UIBackground.SetActive(true);
            ItemManager.Instance.GetComponent<InventoryFold>().IsFolded = false;
            NoteManager.Instance.Show();
        }
    }
}
