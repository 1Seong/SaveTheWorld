using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundAlphaControl : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        
        GetComponent<Image>().DOFade(0.7f, 0.3f).SetUpdate(true);
    }

    public void Hide()
    {
        GetComponent<Image>().DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

    }
}
