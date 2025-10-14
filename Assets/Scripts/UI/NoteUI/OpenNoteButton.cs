using UnityEngine;
using UnityEngine.UI;

public class OpenNoteButton : MonoBehaviour
{
    [SerializeField] private BackgroundAlphaControl background;

    public void OnClick()
    {
        if (NoteManager.Instance.isActive) // close
        {
            background.Hide();
            NoteManager.Instance.Hide();
        }
        else // open
        {
            background.Show();
            ItemManager.Instance.GetComponent<InventoryFold>().IsFolded = false;
            NoteManager.Instance.Show();
        }
    }
}
