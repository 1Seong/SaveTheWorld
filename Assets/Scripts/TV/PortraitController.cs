using UnityEngine;

public class PortraitController : MonoBehaviour
{
    public UnityEngine.UI.Image leftImage;
    public UnityEngine.UI.Image rightImage;
    public float hideAlpha = 0.7f;

    public Sprite GetSprite(string character)
    {
        // 예: "Portraits/Player/Smile"
        string path = $"Assets/Portraits/{character}";
        return Resources.Load<Sprite>(path);
    }

    public void UpdatePortrait(bool? isLeftSpeaker, string leftCharacter, string rightCharacter)
    {
        if (leftCharacter != "")
            leftImage.sprite = GetSprite(leftCharacter);
        else
            leftImage.sprite = null;

        if(rightCharacter != "")
            rightImage.sprite = GetSprite(rightCharacter);
        else
            rightImage.sprite = null;

        if (isLeftSpeaker == null) // narrative
        {
            leftImage.color = new Color(1, 1, 1, hideAlpha);
            rightImage.color = new Color(1, 1, 1, hideAlpha);
        }
        else if (isLeftSpeaker.Value) // left speaker
        {
            // 말하고 있으므로 강조 효과 예시 (원하는 스타일로 변경)
            leftImage.color = Color.white;
            rightImage.color = new Color(1, 1, 1, hideAlpha);
        }
        else // right speaker
        {
            leftImage.color = new Color(1, 1, 1, hideAlpha);
            rightImage.color = Color.white;
        }
    }
}
