using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RCUI : MonoBehaviour, ISaveable
{
    public string SaveKey => "RCUI";

    [SerializeField] TV tv;
    [SerializeField] GameObject remoteCanvas;

    public bool matched = false;
    public bool isActing = false;

    private int idx = 0;
    private string _nums = null;
    public string Nums
    {
        get => _nums;
        set
        {
            _nums = value;
            displayNum(value[^1]);

            ++idx;
            if(idx == 3)
            {
                if (isMatched())
                {
                    matched = true;
                    Invoke(nameof(matchedEffect), 0.3f);
                    tv.channelOnCallBack();
                }
                else
                {
                    Invoke(nameof(notMatchedEffect), 0.3f);
                    _nums = null;
                    idx = 0;
                }
            }
        }
    }

    [SerializeField] private TextMeshProUGUI[] tmps;

    private void Awake()
    {
        SaveManager.Instance.Register(this);

        /*
        for(int i = 0; i < tmps.Length; i++)
        {
            tmps[i].transform.DOMoveY(tmps[i].transform.position.y + 1f, 0f);
        }
        */
    }

    private void OnDestroy()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);
    }

    [System.Serializable]
    private class RCUIData
    {
        public bool rcData;
    }

    public string Save()
    {
        var d = new RCUIData() { rcData = matched };

        return JsonUtility.ToJson(d);
    }

    public void Load(string json)
    {
        try
        {
            var d = JsonUtility.FromJson<RCUIData>(json);

            matched = d.rcData;

            if(matched)
                tv.channelOnCallBack();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"RCUI.Load failed: {e.Message}");
        }
    }

    public void EnableUI()
    {
        remoteCanvas.SetActive(true);
        ItemManager.Instance.gameObject.SetActive(false);
        NoteManager.Instance.TurnOff();
    }

    public void DisableUI()
    {
        remoteCanvas.SetActive(false);
        ItemManager.Instance.gameObject.SetActive(true);
        NoteManager.Instance.TurnOn();
    }

    private void displayNum(char c)
    {
        isActing = true;
        var i = idx;

        tmps[i].transform.DOMoveY(tmps[i].transform.position.y - 0.3f, 0f);
        tmps[i].alpha = 0f;
        tmps[i].text = c.ToString();
        StartCoroutine(FadeTMP(tmps[i], 1f, 0.3f));
        tmps[i].transform.DOMoveY(tmps[i].transform.position.y, 0.3f).OnComplete(() =>
        {
            if(i != 2)
                isActing = false;
        });
    }

    private bool isMatched()
    {
        return _nums == "625";
    }

    private void matchedEffect()
    {
        for (int i = 0; i != 3; ++i)
        {
            tmps[i].color = Color.green;
        }
    }

    private void notMatchedEffect()
    {
        isActing = true;
        for(int i = 0; i != 3; ++i)
        {
            var ci = i;

            tmps[i].color = Color.red;
            StartCoroutine(FadeTMP(tmps[i], 0f, 0.3f));
            tmps[i].transform.DOMoveY(tmps[i].transform.position.y + 0.3f, 0.3f).OnComplete(() =>
            {
                tmps[ci].transform.DOMoveY(tmps[ci].transform.position.y - 0.3f, 0f);
                tmps[ci].color = Color.white;
                tmps[ci].text = "";
                isActing = false;
            });
        }
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
