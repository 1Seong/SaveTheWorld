using UnityEngine;

public class OpenNoteButton : MonoBehaviour
{
    [SerializeField] private BackgroundAlphaControl background;
    [SerializeField] private GameObject UIBackground;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;

    public void OnClick()
    {
        if (NoteManager.Instance.isActive) // close
        {
            upArrow.SetActive(true);
            downArrow.SetActive(false);

            background.Hide();
            UIBackground.SetActive(false);
            NoteManager.Instance.Hide();
        }
        else // open
        {
            upArrow.SetActive(false);
            downArrow.SetActive(true);

            background.Show();
            UIBackground.SetActive(true);
            ItemManager.Instance.GetComponent<InventoryFold>().IsFolded = false;
            NoteManager.Instance.Show();
        }
    }
}
