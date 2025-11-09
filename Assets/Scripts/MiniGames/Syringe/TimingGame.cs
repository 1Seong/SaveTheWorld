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

        if(urpData.TryGetRendererFeature(out rf))
            rf.SetActive(true);
        
        bodyMat.color = Color.red;
        MoveBar();
    }

    private void OnDisable()
    {
        FullScreenPassRendererFeature rf;

        if (urpData.TryGetRendererFeature(out rf))
            rf.SetActive(false);

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
        timingBar.DOAnchorPosX(targetDis, moveDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        cameraTransform.DOShakePosition(0.4f, 0.1f).OnComplete(() =>
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
                timingBar.anchoredPosition = new Vector2(-0.4f, 0f);
                targetZone.anchoredPosition = new Vector2(Random.Range(-targetZoneMaxDis, targetZoneMaxDis), 0f);
                targetZone.sizeDelta = new Vector2(targetZoneWidths[successCount], 1f);
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
                string[] texts = null;
                texts.Append(savedCount.ToString());
                MiniGameManager.instance.GameEnd(texts);
            }
            else if (savedCount == 5)
                MiniGameManager.instance.GameEnd(2);

        }
    }
}
