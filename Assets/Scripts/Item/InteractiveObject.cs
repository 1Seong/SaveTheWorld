using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class InteractiveObject : MonoBehaviour
{
    public Item.Interactives typeId;

    [Header("# Blurr Setting")]
    [SerializeField] private bool hasBlurr = false;

    [Header("# Drop Item Setting")]
    [SerializeField] private bool hasDropItem = false; // does this interactible "drops" item ?   Do not confuse with the ones that have "attached" itemObject
    [SerializeField] private float dropDisX = 0f;

    [Header("# Toggle Item Setting")]
    [SerializeField] private bool hasToggleItem = false;

    [Header("# Close up Setting")]
    [SerializeField] private bool hasCloseUp = false;
    [SerializeField] private float closeUpDistance = 5f;
    [SerializeField] private bool hasMiniGame = false;
    [SerializeField] private string SceneName;
    [SerializeField] private UnityAction ReturnFromCloseUpUnityAction;


    private void Start()
    {
        if (hasBlurr && Item.IsBlurred(typeId))
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;

        if(hasDropItem && Item.IsDropped(typeId))
            DropInit();

        NoteManager.Instance.BlurrUnlockEvent += unlockBlurr;
        ItemManager.Instance.ReturnFromCloseUpEvent += buttonInteractiveOn;
    }

    private void OnDestroy()
    {
        NoteManager.Instance.BlurrUnlockEvent -= unlockBlurr;
        ItemManager.Instance.ReturnFromCloseUpEvent -= buttonInteractiveOn;
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

    public void DropItemOnClick()
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

    public void ToggleItemOnClick()
    {
        if(!hasToggleItem) return;
        if (GetComponentInChildren<ItemObject>(true) == null) return;

        var itemObject = GetComponentInChildren<ItemObject>(true).gameObject;
        itemObject.SetActive(!itemObject.activeSelf);
    }

    public void CloseUpOnClick()
    {
        if(!hasCloseUp) return;

        ItemManager.Instance.TurnOffGoButtons();
        ItemManager.Instance.TurnOnReturnFromCloseUpButton(); // should turn off when entering minigame

        var targetPos = transform.position;
        var res = StageManager.Instance.CurrentPlaneId switch
        {
            0 => new Vector3(targetPos.x, targetPos.y, closeUpDistance),
            1 => new Vector3(-closeUpDistance, targetPos.y, targetPos.z),
            2 => new Vector3(targetPos.x, targetPos.y, -closeUpDistance),
            3 => new Vector3(closeUpDistance, targetPos.y, targetPos.z),
            4 => new Vector3(targetPos.x, closeUpDistance, targetPos.z),
            _ => throw new System.NotImplementedException()
        };


        Camera.main.transform.DOMove(res, 0.5f);

        // TODO : implement minigame enter logic
    }

    private void buttonInteractiveOn()
    {
        GetComponent<Button>().interactable = true;
    }
}
