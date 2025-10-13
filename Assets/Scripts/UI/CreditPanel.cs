using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditPanel : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private GameObject panel;

    public void Show()
    {
        background.gameObject.SetActive(true);
        background.DOFade(0.7f, 0.3f).SetUpdate(true);

        panel.SetActive(true);
    }

    public void Hide()
    {
        background.DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            background.gameObject.SetActive(false);
        });

        panel.SetActive(false);
    }
}
