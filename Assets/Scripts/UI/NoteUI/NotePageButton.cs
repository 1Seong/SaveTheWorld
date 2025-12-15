using UnityEngine;

public class NotePageButton : MonoBehaviour
{
    public bool isCover = false;

    public void OnClick()
    {
        if(isCover)
            AudioManager.Instance.PlaySfx(AudioType.SFX_Note_Cover);
        else
            AudioManager.Instance.PlaySfx(AudioType.SFX_Note_Page);
    }
}
