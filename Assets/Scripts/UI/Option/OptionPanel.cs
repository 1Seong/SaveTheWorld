using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public static event Action<float> MouseSensitivityOnValueChangedEvent;

    [SerializeField] private Image background;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool isActive = false;

    private static OptionPanel _instance;
    public static OptionPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindFirstObjectByType<OptionPanel>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("OptionPanel");
                    _instance = singletonObj.AddComponent<OptionPanel>();
                    DontDestroyOnLoad(singletonObj);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOnClick();
        }
    }

    public void OpenOnClick()
    {
        if (isActive)
            Hide();
        else
            Show();
    }

    public void MouseSensitivityOnValueChanged(float value)
    {
        var realValue = Mathf.Lerp(0f, 4f, value);

        MouseSensitivityOnValueChangedEvent?.Invoke(realValue);
    }

    public void Show()
    {
        background.gameObject.SetActive(true);
        isActive = true;
        GameManager.Instance.StopTime();
        background.DOFade(0.7f, 0.3f).SetUpdate(true);

        panel.SetActive(true);
    }

    public void Hide()
    {
        isActive = false;
        GameManager.Instance.ResumeTime();
        background.DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            background.gameObject.SetActive(false);
        });

        panel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        // ItemManager.Instance.SaveItems();
        Hide();
        SceneTransition.Instance.LoadScene("MainMenu");
    }
}
