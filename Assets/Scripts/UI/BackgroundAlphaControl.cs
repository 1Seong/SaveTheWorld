using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundAlphaControl : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);

        Image img;
        SpriteRenderer spriteRenderer;
        if(TryGetComponent(out img))
            GetComponent<Image>().DOFade(0.7f, 0.3f).SetUpdate(true);
        else if(TryGetComponent(out spriteRenderer))
            GetComponent<SpriteRenderer>().DOFade(0.7f, 0.3f).SetUpdate(true);
    }

    public void Hide()
    {
        Image img;
        SpriteRenderer spriteRenderer;
        if (TryGetComponent(out img))
            GetComponent<Image>().DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        else if (TryGetComponent(out spriteRenderer))
            GetComponent<SpriteRenderer>().DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
