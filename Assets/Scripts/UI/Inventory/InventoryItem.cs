using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{

    [SerializeField] private ItemData _data;

    private RectTransform item;
    private RectTransform itemTemp;

    public ItemData Data
    {
        get => _data;
        set
        {
            _data = value;
            transform.GetChild(0).GetComponent<Image>().sprite = _data.sprite;
        }
    }

    private void Start()
    {
        item = transform.GetChild(0) as RectTransform;

        
    }

    private void OnDestroy()
    {
        
    }

    private void activateItemFollow()
    {
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
        Destroy(itemTemp.gameObject);
        itemTemp = null;
        item.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        activateItemFollow();
        ItemManager.Instance.IsHolding = true;
        ItemManager.Instance.SelectedItem = this;
    }
}
