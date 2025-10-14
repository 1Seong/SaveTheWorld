using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Item.Interactives typeId;

    [Header("# Blurr Setting")]
    [SerializeField] private bool hasBlurr = false;

    [Header("# Drop Item Setting")]
    [SerializeField] private bool hasDropItem = false; // does this interactible "drops" item ?   Do not confuse with the ones that have "attached" itemObject
    [SerializeField] private float dropDisX = 0f;

    [Header("# Attached Item Setting")]
    [SerializeField] private bool hasAttachedItem = false;


    private void Start()
    {
        if (hasBlurr && Item.IsBlurred(typeId))
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;

        if(hasDropItem && Item.IsDropped(typeId))
            DropInit();

        NoteManager.Instance.BlurrUnlockEvent += unlockBlurr;
    }

    private void OnDestroy()
    {
        NoteManager.Instance.BlurrUnlockEvent -= unlockBlurr;
    }

    private void unlockBlurr(Item.Interactives id)
    {
        if (id != typeId) return;

        Item.UnlockBlurr(typeId);
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Button>().interactable = true;
    }

    private void DropInit()
    {
        if (GetComponentInChildren<ItemObject>() == null) return; // item is already collected

        var itemObject = GetComponentInChildren<ItemObject>().gameObject;
        itemObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(dropDisX, -5f), 0.5f).SetUpdate(true).OnComplete(() =>
        {
            itemObject.SetActive(true);
        });
    }

    public void DropItem()
    {
        if (!hasDropItem) return;
        if(Item.IsDropped(typeId)) return;
        if (GetComponentInChildren<ItemObject>(true) == null) return;

        Item.Drop(typeId);
        var itemObject = GetComponentInChildren<ItemObject>(true).gameObject;
        itemObject.SetActive(true);

        Sequence seq = DOTween.Sequence(itemObject);
        seq.Join(itemObject.GetComponent<RectTransform>().DOAnchorPosY(-5f, 1f).SetUpdate(true).SetEase(Ease.OutBounce))
            .Join(itemObject.GetComponent<RectTransform>().DOAnchorPosX(dropDisX, 1f).SetUpdate(true).SetEase(Ease.OutCirc));
    }

    public void ControlAttached()
    {
        if(!hasAttachedItem) return;
        if (GetComponentInChildren<ItemObject>(true) == null) return;

        var itemObject = GetComponentInChildren<ItemObject>(true).gameObject;
        itemObject.SetActive(!itemObject.activeSelf);
    }
}
