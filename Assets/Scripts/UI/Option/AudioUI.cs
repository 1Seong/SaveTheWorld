using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image targetIamge;
    [SerializeField] Sprite muteSprite;
    [SerializeField] Sprite normalSprite;
    bool isMute = false;

    public void OnClick()
    {
        if (isMute) // mute -> normal
        {
            isMute = false;
            slider.interactable = true;
            targetIamge.sprite = normalSprite;
        }
        else // normal -> mute
        {
            isMute = true;
            slider.interactable = false;
            targetIamge.sprite = muteSprite;
        }
    }
}