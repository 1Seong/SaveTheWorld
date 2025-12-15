using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public static event Action OnInventoryClickTutorialEvent;


    [SerializeField] private ItemData _data;

    private RectTransform item;
    private RectTransform itemTemp;

    public ItemData Data
    {
        get => _data;
        set
        {
            _data = value;
            transform.GetChild(0).localScale = Vector3.one * _data.targetScale;
            transform.GetChild(0).GetComponent<Image>().sprite = _data.sprite;
            transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        }
    }

    private void Start()
    {
        item = transform.GetChild(0) as RectTransform;

        
    }

    private void activateItemFollow()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_InvItemOn);

        var screenPosC = RectTransformUtility.WorldToScreenPoint(null, item.position);
        
        Vector2 localPosC;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponentInParent<Canvas>().transform as RectTransform, screenPosC, null, out localPosC);

        item.gameObject.SetActive(false);
        itemTemp = Instantiate(item, NoteManager.Instance.transform);
        itemTemp.anchoredPosition = localPosC;
        itemTemp.GetComponent<FollowMouse>().enabled = true;
        itemTemp.gameObject.SetActive(true);
        
    }

    public void DeactivateItemFollow()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_InvItemOff);

        Destroy(itemTemp.gameObject);
        itemTemp = null;
        item.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (Data.id == Item.Items.Controller)
        {
            Camera.main.GetComponentInChildren<RCUI>(true).EnableUI();
            return;
        }
        else if(Data.id == Item.Items.FilledBottle)
        {
            Camera.main.GetComponentInChildren<BottleUI>(true).EnableUI();
            return;
        }
        else if(Data.id == Item.Items.LetterJu)
        {
            OnInventoryClickTutorialEvent?.Invoke();
        }

        activateItemFollow();
        ItemManager.Instance.IsHolding = true;
        ItemManager.Instance.SelectedItem = this;
    }
}
