using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float gustChancePerSec = 0.1f;
    public float minGust = 0.5f;
    public float maxGust = 1.5f;
    public float gustDurationMin = 0.2f;
    public float gustDurationMax = 0.8f;

    public float gustIncrementStep = 0.05f;

    [SerializeField] private bool isActing = false;

    void Start()
    {
        StartCoroutine(GustRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(GustRoutine());
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        minGust += gustIncrementStep * Time.deltaTime;
        maxGust += gustIncrementStep * Time.deltaTime;
    }

    IEnumerator GustRoutine()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            while (GameManager.Instance.IsPlaying && MiniGameManager.instance.IsPlaying)
            {
                if (!isActing)
                {
                    isActing = true;

                    float gust = Random.Range(minGust, maxGust);
                    float dir = Random.value < 0.5f ? -1f : 1f;
                    float dur = Random.Range(gustDurationMin, gustDurationMax);
                    StartCoroutine(ApplyGust(gust * dir, dur));
                }
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator ApplyGust(float strength, float duration)
    {
        float t = 0f;
        float initial = BalanceGame.instance.WindForce.x;
        while (t < duration)
        {
            float target = initial + strength;
            BalanceGame.instance.WindForce = new Vector2(target, 0f);
            t += Time.deltaTime;
            yield return null;
        }
        // 자연 복귀 (간단하게)
        BalanceGame.instance.WindForce = Vector2.zero;
        isActing = false;
    }
}
