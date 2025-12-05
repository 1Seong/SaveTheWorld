using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimingGame : MonoBehaviour
{
    public RectTransform timingBar;
    public RectTransform targetZone;
    public Image background;
    public Transform body;
    public Material bodyMat;
    public Image targetZoneImage;
    public Transform syringe;
    public Transform cameraTransform;
    public UniversalRendererData urpData;
    public Material FullScreenShaderMat;
    public Transform TowelParent;
    public Vector3[] bodyTransforms;
    public float[] moveDurations;
    public float[] targetZoneWidths;

    public float moveDuration = 2f; // 바 한 왕복 시간
    public int successCount = 0;
    public float targetDis = 0.4f;
    public float targetZoneMaxDis = 0.4f;
    public float syringeTargetDis = 0.1f;

    private int savedCount = 0;

    void Start()
    {
        FullScreenPassRendererFeature rf;

        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = FullScreenShaderMat;
            rf.SetActive(true);
        }

        bodyMat.color = Color.red;
        MoveBar();
    }

    private void OnDisable()
    {
        FullScreenPassRendererFeature rf;

        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = null;
            rf.SetActive(false);
        }

        bodyMat.color = Color.red;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            CheckTiming();
    }

    void MoveBar()
    {
        timingBar.DOAnchorPosX(targetDis, moveDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart);
    }

    void StopBar()
    {
        timingBar.DOKill();
    }

    void CheckTiming()
    {
        float barX = timingBar.anchoredPosition.x;
        float zoneMin = targetZone.anchoredPosition.x - (targetZone.localScale.x * targetZone.sizeDelta.x) / 2f;
        float zoneMax = targetZone.anchoredPosition.x + (targetZone.localScale.x * targetZone.sizeDelta.x) / 2f;

        MiniGameManager.instance.IsPlaying = false;
        StopBar();

        if (barX >= zoneMin && barX <= zoneMax)
        {
            SuccessFeedback();
        }
        else
        {
            FailFeedback();
        }
    }

    void SuccessFeedback()
    {
        ++savedCount;
        targetZoneImage.color = Color.green;
        bodyMat.color = Color.green;

        syringe.DOLocalMoveZ(syringe.transform.localPosition.z + syringeTargetDis, 0.8f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            syringe.DOLocalMoveZ(syringe.transform.localPosition.z - syringeTargetDis, 0.8f);
            ResetGame();
        });
    }

    void FailFeedback()
    {
        targetZoneImage.color = Color.red;
        cameraTransform.DOShakePosition(0.3f, 0.1f, 20).OnComplete(() =>
        {
            ResetGame();
        });
    }

    void ResetGame()
    {
        //MiniGameManager.instance.CountUp();
        ++successCount;

        if(successCount < 5)
        {
            background.DOFade(1f, 1f).OnComplete(() =>
            {
                body.localPosition = bodyTransforms[successCount];
                bodyMat.color = Color.red;
                timingBar.anchoredPosition = new Vector2(-1f, 0f);
                targetZone.anchoredPosition = new Vector2(Random.Range(-targetZoneMaxDis, targetZoneMaxDis), 0f);
                targetZone.sizeDelta = new Vector2(targetZoneWidths[successCount], 0.3f);
                targetZoneImage.color = Color.yellow;
                moveDuration = moveDurations[successCount];
                TowelParent.GetChild(successCount-1).gameObject.SetActive(false);
                TowelParent.GetChild(successCount).gameObject.SetActive(true);

                background.DOFade(0f, 1f);
                MoveBar();
                MiniGameManager.instance.IsPlaying = true;
            });
            
        }
        else
        {
            if(savedCount == 0)
            {
                MiniGameManager.instance.GameEnd(0);
            }
            else if (savedCount > 0 && savedCount < 5)
            {
                string[] texts = new string[3];

                texts[0] = $"부상당한 {savedCount}명의 전우의 목숨을 구했었어.";
                texts[1] = $"하지만 {5 - savedCount}명은 그러지 못했지.";
                texts[2] = "난 그렇게 죄책감을 안고...";
                MiniGameManager.instance.GameEnd(texts);
            }
            else if (savedCount == 5)
                MiniGameManager.instance.GameEnd(2);

        }
    }
}
