using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour, ISaveable
{
    public string SaveKey => "TutorialManager";

    public static TutorialManager Instance;

    public bool isTutorialCleared = false;
    [SerializeField] int stepNum = 1;

    public GameObject fullRaycastBlockImage; // click nothing
    public GameObject roomRaycastBlockImage; // can access ui
    public Image background;

    [Header("Step1 - Plane A")]
    public float step1WaitingTime = 3f;
    public float step1FadeTime = 0.8f;
    public float step1TextShowDuration = 3f;
    public TextMeshProUGUI step1TMP;

    [Header("Step2 - Plane B")]
    public float step2WaitingTime = 3f;
    public float step2FadeTime = 0.8f;
    public float step2TextShowDuration = 3f;
    public TextMeshProUGUI step2TMP;

    [Header("Step3 - Plane C")]
    public float step3WaitingTime = 3f;
    public float step3FadeTime = 0.8f;
    public float step3TextShowDuration = 3f;
    public GameObject step3confusedTMPParent;
    public TextMeshProUGUI step3TMP;

    [Header("Step4 - Plane D")]
    public float step4WaitingTime = 3f;
    public float step4FadeTime = 0.8f;
    public float step4TextShowDuration = 3f;
    public TextMeshProUGUI step4TMP;
    public GameObject doorHighlightImage;
    public GameObject LetterHighlightImage;
    public GameObject NoteHighlightImage;
    private bool isDoorClicked = false;

    [Header("Step5 - Note")]
    public float step5WaitingTime = 3f;
    public float step5FadeTime = 0.8f;
    public float step5TextShowDuration = 3f;
    public TextMeshProUGUI step5TMP;
    public GameObject nextPageHighlightImage;
    public GameObject InventoryHighightImage;
    public GameObject letterTargetHighlightImage;
    public GameObject syringeHighlightImage;
    public ParticleSystem syringeParticle;
    public OpenNoteButton noteButton;

    void Awake()
    {
        SaveManager.Instance.Register(this);

        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    [System.Serializable]

    private class TutorialManagerData
    {
        public bool cleared;
    }

    public string Save()
    {
        var d = new TutorialManagerData() { cleared = isTutorialCleared };

        return JsonUtility.ToJson(d);
    }

    public void Load(string json)
    {
        try
        {
            var d = JsonUtility.FromJson<TutorialManagerData>(json);

            isTutorialCleared = d.cleared;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"TutorialManager.Load failed: {e.Message}");
        }
    }

    private void Start()
    {
        InventoryItem.OnInventoryClickTutorialEvent += step5InventoryOnClick;
        LetterTarget.tutorialEvent += step5LetterTargetOnClick;
    }

    public void StartTutorial()
    {
        if (!isTutorialCleared)
        {
            roomRaycastBlockImage.SetActive(true);
            ItemManager.Instance.TurnOffGoButtons();
            NoteManager.Instance.TurnOff();

            Invoke(nameof(step1Enter), step1WaitingTime);
        }
    }

    private void OnDestroy()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);

        background.material.SetFloat("_DissolveStrength", 1f);

        InventoryItem.OnInventoryClickTutorialEvent -= step5InventoryOnClick;
        LetterTarget.tutorialEvent -= step5LetterTargetOnClick;
    }

    private void SetTutorialCleared()
    {
        isTutorialCleared = true;
    }

    // ========== Step 1 - Plane A ==========
    // Entry Point : wait from start
    // wait
    // fade out and show text
    // activate right navigation button
    private void step1Enter()
    {
        step1TMP.gameObject.SetActive(true);
        StartCoroutine(FadeTMP(step1TMP, 1f, step1FadeTime));
        background.gameObject.SetActive(true);
        background.material.DOFloat(0f, "_DissolveStrength", step1FadeTime);
        background.DOFade(1f, step1FadeTime).OnComplete(() =>
        {
            Invoke(nameof(step1FadeOut), step1TextShowDuration);
        });
    }

    private void step1FadeOut()
    {
        StartCoroutine(FadeTMP(step1TMP, 0f, step1FadeTime));
        background.material.DOFloat(1f, "_DissolveStrength", step1FadeTime);
        background.DOFade(0f, step1FadeTime).OnComplete(() =>
        {
            step1TMP.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            ++stepNum;
            ItemManager.Instance.TurnOnRightButton();
        });
    }

    // ========== step2 - Plane B ==========
    // Entry Point : click right nav button
    // wait ( cannot go right )
    // fade out and show text
    // go right

    public void step2OnClick()
    {
        if (isTutorialCleared || stepNum != 2) return;

        fullRaycastBlockImage.SetActive(true);
        Invoke(nameof(step2FadeIn), step2WaitingTime);
    }

    private void step2FadeIn()
    {
        step2TMP.gameObject.SetActive(true);
        StartCoroutine(FadeTMP(step2TMP, 1f, step2FadeTime));
        background.gameObject.SetActive(true);
        background.material.DOFloat(0f, "_DissolveStrength", step2FadeTime);
        background.DOFade(1f, step2FadeTime).OnComplete(() =>
        {
            Invoke(nameof(step2FadeOut), step2TextShowDuration);
        });
    }

    private void step2FadeOut()
    {
        StartCoroutine(FadeTMP(step2TMP, 0f, step2FadeTime));
        background.material.DOFloat(1f, "_DissolveStrength", step2FadeTime);
        background.DOFade(0f, step2FadeTime).OnComplete(() =>
        {
            step2TMP.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            ++stepNum;
            fullRaycastBlockImage.SetActive(false);
        });
    }

    // ========== step3 - Plane C ==========
    // Entry Point : click right nav button
    // wait ( cannot go right )
    // fade out and show text
    // go right

    public void step3OnClick()
    {
        if (isTutorialCleared || stepNum != 3) return;

        fullRaycastBlockImage.SetActive(true);
        Invoke(nameof(step3FadeIn), step3WaitingTime);
    }

    private void step3FadeIn()
    {
        background.gameObject.SetActive(true);
        background.material.DOFloat(0f, "_DissolveStrength", step3FadeTime);
        background.DOFade(1f, step3FadeTime).OnComplete(() =>
        {
            StartCoroutine(step3ShowConfusedTextsCoroutine());
        });
    }

    IEnumerator step3ShowConfusedTextsCoroutine()
    {
        var tmps = step3confusedTMPParent.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach(var i in tmps)
        {
            i.gameObject.SetActive(true);
            StartCoroutine(FadeTMP(i, 1f, step3FadeTime));
            yield return new WaitForSeconds(step3FadeTime - 0.3f);
        }
        yield return new WaitForSeconds(step3TextShowDuration);
        foreach (var i in tmps)
        {
            StartCoroutine(FadeTMP(i, 0f, step3FadeTime));
            yield return new WaitForSeconds(step3FadeTime - 0.3f);
            //i.gameObject.SetActive(false);
        }

        step3TMP.gameObject.SetActive(true);
        StartCoroutine(FadeTMP(step3TMP, 1f, step3FadeTime));

        Invoke(nameof(step3FadeOut), step3FadeTime + step3TextShowDuration);
    }

    private void step3FadeOut()
    {
        StartCoroutine(FadeTMP(step3TMP, 0f, step3FadeTime));
        background.material.DOFloat(1f, "_DissolveStrength", step3FadeTime);
        background.DOFade(0f, step3FadeTime).OnComplete(() =>
        {
            step3TMP.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            ++stepNum;
            fullRaycastBlockImage.SetActive(false);
        });
    }

    // ========== step4 - Plane D ==========
    // Entry Point : click right nav button
    // highlight door
    // click door
    // fade out and show text
    // highlight letter 'ju'
    // collect letter 'ju'
    // activate note

    public void step4OnClick()
    {
        if (isTutorialCleared || stepNum != 4) return;

        fullRaycastBlockImage.SetActive(true);
        Invoke(nameof(step4highlightDoor), 1f);
    }

    private void step4highlightDoor()
    {
        doorHighlightImage.SetActive(true);
        fullRaycastBlockImage.SetActive(false);
        roomRaycastBlockImage.SetActive(false);
    }

    // Entry Point : Click door
    public void step4DoorOnClick()
    {
        if (isTutorialCleared || stepNum != 4 || isDoorClicked) return;

        isDoorClicked = true;
        step4FadeIn();
    }

    private void step4FadeIn()
    {
        doorHighlightImage.SetActive(false);
        step4TMP.gameObject.SetActive(true);
        StartCoroutine(FadeTMP(step4TMP, 1f, step4FadeTime));
        background.gameObject.SetActive(true);
        background.material.DOFloat(0f, "_DissolveStrength", step4FadeTime);
        background.DOFade(1f, step4FadeTime).OnComplete(() =>
        {
            Invoke(nameof(step4FadeOut), step4TextShowDuration);
        });
    }

    private void step4FadeOut()
    {
        LetterHighlightImage.SetActive(true);

        StartCoroutine(FadeTMP(step4TMP, 0f, step4FadeTime));
        background.material.DOFloat(1f, "_DissolveStrength", step4FadeTime);
        background.DOFade(0f, step4FadeTime).OnComplete(() =>
        {
            step4TMP.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        });
    }

    // Entry Point : Click Letter 'Ju'
    public void step4LetterOnClick()
    {
        if (isTutorialCleared || stepNum != 4) return;

        NoteManager.Instance.TurnOn();
        NoteHighlightImage.SetActive(true);
        LetterHighlightImage.SetActive(false);

        ++stepNum;
    }

    // ========== step5 - Note ==========
    // Entry Point : click note open button
    // highlight next page button
    // highlight inventory item
    // highlight letter target
    // insert letter
    // close note
    // repetitively show light image on syringe
    // fade out and show text
    // tutorial finish

    public void step5OnClick()
    {
        if(isTutorialCleared || stepNum != 5) return;

        NoteHighlightImage.SetActive(false);
        nextPageHighlightImage.SetActive(true);
    }

    // Entry Point : click next page button
    public void step5NextPageOnClick()
    {
        if (isTutorialCleared || stepNum != 5) return;

        nextPageHighlightImage.SetActive(false);
        InventoryHighightImage.SetActive(true);
    }

    // Entry Point : click inventory item
    public void step5InventoryOnClick()
    {
        if (isTutorialCleared || stepNum != 5) return;

        InventoryHighightImage.SetActive(false);
        letterTargetHighlightImage.SetActive(true);
    }

    // Entry point : click letter target
    public void step5LetterTargetOnClick()
    {
        if (isTutorialCleared || stepNum != 5) return;

        letterTargetHighlightImage.SetActive(false);
        fullRaycastBlockImage.SetActive(true);
        noteButton.OnClick();
        syringeHighlightImage.SetActive(true);

        FadeParticleAlphaTween(syringeParticle, 1f, 0f, 1f).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
        {
            syringeHighlightImage.SetActive(false);
            step5FadeIn();
        });
    }

    private void step5FadeIn()
    {
        step5TMP.gameObject.SetActive(true);
        StartCoroutine(FadeTMP(step5TMP, 1f, step5FadeTime));
        background.gameObject.SetActive(true);
        background.material.DOFloat(0f, "_DissolveStrength", step5FadeTime);
        background.DOFade(1f, step5FadeTime).OnComplete(() =>
        {
            Invoke(nameof(step5FadeOut), step5TextShowDuration);
        });
    }

    private void step5FadeOut()
    {
        fullRaycastBlockImage.SetActive(false);
        ItemManager.Instance.TurnOnGoButtons();
        SetTutorialCleared();

        StartCoroutine(FadeTMP(step5TMP, 0f, step5FadeTime));
        background.material.DOFloat(1f, "_DissolveStrength", step5FadeTime);
        background.DOFade(0f, step5FadeTime).OnComplete(() =>
        {
            step5TMP.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        });
    }

    public void particleFadeDebugOnClick()
    {
        FadeParticleAlphaTween(syringeParticle, 1f, 0f, 1f).SetLoops(6, LoopType.Yoyo);
    }

    // ========== utility ==========

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

    private Tween FadeParticleAlphaTween(ParticleSystem ps, float from, float target, float duration)
    {
        float value = from;

        return DOTween.To(() => value, v => value = v, target, duration)
            .OnUpdate(() =>
            {
                SetParticleAlpha(ps, value, value);
            });
            
    }

    private void SetParticleAlpha(ParticleSystem ps, float alpha0, float alpha80)
    {
        var col = ps.colorOverLifetime;
        col.enabled = true;

        // 기존 Gradient 가져오기 (복사 필요)
        Gradient g = col.color.gradient;

        // 기존 AlphaKeys 가져오기
        var alphaKeys = g.alphaKeys;

        // 0% 지점 수정
        alphaKeys[0].alpha = alpha0;

        // 80% 지점 수정
        // 0.8에 대응하는 키를 찾아 수정하거나,
        // 없으면 새로 추가
        bool found80 = false;
        for (int i = 0; i < alphaKeys.Length; i++)
        {
            if (Mathf.Approximately(alphaKeys[i].time, 0.8f))
            {
                alphaKeys[i].alpha = alpha80;
                found80 = true;
                break;
            }
        }

        if (!found80)
        {
            Array.Resize(ref alphaKeys, alphaKeys.Length + 1);
            alphaKeys[alphaKeys.Length - 1] = new GradientAlphaKey(alpha80, 0.8f);
        }

        // 변경된 키 다시 할당
        g.alphaKeys = alphaKeys;

        // 변경된 Gradient 재할당
        col.color = new ParticleSystem.MinMaxGradient(g);
    }
}
