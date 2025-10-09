using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InventoryFold : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private RectTransform targetObject;
    [SerializeField] private Button targetButton;
    [SerializeField] private bool _isFolded = false;
    
    public bool IsFolded
    {
        get => _isFolded;
        set
        {
            _isFolded = value;
            if(value) // Fold
            {
                Fold();
            }
            else // Unfold
            {
                UnFold();
            }
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
