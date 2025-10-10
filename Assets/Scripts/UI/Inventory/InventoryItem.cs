using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;
    public ItemData Data
    {
        get => _data;
        set
        {
            _data = value;
            transform.GetChild(0).GetComponent<Image>().sprite = _data.sprite;
        }
    }

    private void OnEnable()
    {
        
    }
}
