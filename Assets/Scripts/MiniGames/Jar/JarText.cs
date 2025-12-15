using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JarText : MonoBehaviour
{
    TextMeshProUGUI tmp;
    Image img;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        img = GetComponentInChildren<Image>();
    }

    public void Show()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_J_Show);

        StartCoroutine(FadeTMP(tmp, 1f, 1f));
        img.DOFade(0.31f, 1f);
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
}
