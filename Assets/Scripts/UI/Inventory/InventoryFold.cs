using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryFold : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private RectTransform targetObject;
    [SerializeField] private Button targetButton;
    [SerializeField] private bool _isFolded = false;

    [SerializeField] private float itemAddindicateDur = 0.9f;
    [SerializeField] private float itemAddindicateDis = 15f;
    
    public bool IsFolded
    {
        get => _isFolded;
        set
        {
            if (_isFolded != value)
            {
                if (value) // Fold
                {
                    Fold();
                }
                else // Unfold
                {
                    UnFold();
                }
            }
            _isFolded = value;
        }
    }

    [SerializeField] private bool _isActing = false;
    public bool IsActing
    {
        get => _isActing;
        set
        {
            _isActing = value;
            if(value) // acting
            {
                targetButton.interactable = false;
            }
            else // not acting
            {
                targetButton.interactable = true;
            }
        }
    }

    private void Start()
    {
        ItemManager.Instance.ItemAddedToUIEvent += itemAddIndicate;
    }

    private void OnDestroy()
    {
        ItemManager.Instance.ItemAddedToUIEvent -= itemAddIndicate;
    }

    private void itemAddIndicate()
    {
        if (!IsFolded) return;

        targetObject.DOAnchorPosX(targetObject.anchoredPosition.x - itemAddindicateDis, itemAddindicateDur / 2f).SetUpdate(true).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            targetObject.DOAnchorPosX(targetObject.anchoredPosition.x + itemAddindicateDis, itemAddindicateDur / 2f).SetUpdate(true).SetEase(Ease.InOutSine);
        });
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnButtonClick();
        }
    }

    public void OnButtonClick()
    {
        IsFolded = !IsFolded;
    }

    private void Fold()
    {
        IsActing = true;

        var width = targetObject.rect.width;
        
        targetObject.DOAnchorPosX(targetObject.anchoredPosition.x + width, duration).SetUpdate(true).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            IsActing = false;
        });
    }

    private void UnFold()
    {
        IsActing = true;

        var width = targetObject.rect.width;

        targetObject.DOAnchorPosX(targetObject.anchoredPosition.x - width, duration).SetUpdate(true).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            IsActing = false;
        });
    }
}
