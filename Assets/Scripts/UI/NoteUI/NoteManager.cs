using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public bool isActive = false;

    [SerializeField] private RectTransform noteUIParent;

    private static NoteManager _instance;
    public static NoteManager Instance
    {
        get
        {
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

    public void Show()
    {
        isActive = true;
        noteUIParent.DOAnchorPosY(0f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void Hide()
    {
        isActive = false;
        noteUIParent.DOAnchorPosY(-424f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

}
