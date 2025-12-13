using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager2 : MonoBehaviour
{
    [SerializeField] float playerFadewaitTime = 2f;
    [SerializeField] float backgroundFadeWaitTime = 1.5f;
    [SerializeField] float textWaitTime = 0.5f;
    [SerializeField] SpriteRenderer player1;
    [SerializeField] SpriteRenderer player2;
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image buttonImage;
    [SerializeField] TextMeshProUGUI buttonText;

    private void Start()
    {
        background.DOFade(1f, 0f);

        StartCoroutine(Ending2Coroutine());
    }

    IEnumerator Ending2Coroutine()
    {
        var t1 = background.DOFade(0f, 1.5f);
        yield return t1.WaitForCompletion();

        yield return new WaitForSeconds(playerFadewaitTime);

        var seq = DOTween.Sequence();
        seq.Append(player1.DOFade(0f, 1f));
        seq.Join(player2.DOFade(0f, 1f));
        yield return seq.WaitForCompletion();

        yield return new WaitForSeconds(textWaitTime);

        yield return StartCoroutine(FadeTMP(title, 1f, 1f));
        yield return new WaitForSeconds(backgroundFadeWaitTime);

        var t2 = background.DOFade(1f, 1f);
        var t3 = title.DOColor(Color.black, 1f);
        yield return t2.WaitForCompletion();

        yield return StartCoroutine(FadeTMP(title, 1f, 1f));
        yield return new WaitForSeconds(textWaitTime);

        yield return StartCoroutine(FadeTMP(description, 1f, 1f));
        yield return new WaitForSeconds(textWaitTime);

        buttonImage.DOFade(1f, 1f);
        yield return  StartCoroutine(FadeTMP(buttonText, 1f, 1f));
        
    }

    private IEnumerator FadeTMP(TextMeshProUGUI tmp, float targetAlpha, float duration)
    {
        float startAlpha = tmp.alpha;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            tmp.alpha = newAlpha;
            yield return null;
        }
        tmp.alpha = targetAlpha;
    }

    public void OnClick()
    {
        SceneTransition.Instance.LoadScene("MainMenu");
    }
}
