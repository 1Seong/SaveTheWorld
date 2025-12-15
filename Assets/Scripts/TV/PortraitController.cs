using DG.Tweening;
using UnityEngine;

public class PortraitController : MonoBehaviour
{
    public UnityEngine.UI.Image leftImage;
    public UnityEngine.UI.Image rightImage;
    public float hideAlpha = 0.5f;

    public Sprite GetSprite(string character)
    {
        // 예: "Portraits/Player/Smile"
        Debug.Log(character);
        string path = $"Portraits/{character}";
        return Resources.Load<Sprite>(path);
    }

    public void UpdatePortrait(bool? isLeftSpeaker, string leftCharacter, string rightCharacter)
    {
        if (leftCharacter != "")
        {
            leftImage.sprite = GetSprite(leftCharacter);
            leftImage.gameObject.SetActive(true);
            leftImage.SetNativeSize();
        }
        else
        {
            leftImage.sprite = null;
            leftImage.gameObject.SetActive(false);
        }

        if (rightCharacter != "")
        {
            rightImage.sprite = GetSprite(rightCharacter);
            rightImage.gameObject.SetActive(true);
            rightImage.SetNativeSize();

            if(rightCharacter == "mom_n_sister")
                rightImage.rectTransform.localScale = Vector3.one;
        }
        else
        {
            rightImage.sprite = null;
            rightImage.gameObject.SetActive(false);
        }

        if (isLeftSpeaker == null) // narrative
        {
            leftImage.color = Color.gray;
            rightImage.color = Color.gray;
        }
        else if (isLeftSpeaker.Value) // left speaker
        {
            // 말하고 있으므로 강조 효과 예시 (원하는 스타일로 변경)
            leftImage.color = Color.white;
            rightImage.color = Color.gray;
        }
        else // right speaker
        {
            leftImage.color = Color.gray;
            rightImage.color = Color.white;
        }
    }
}
