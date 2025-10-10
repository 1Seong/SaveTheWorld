using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTarget : MonoBehaviour
{
    [SerializeField] private Item.Items id;

    public void OnInteract(ItemData data)
    {
        if(id == data.id)
        {
            itemMatched();
            Debug.Log("Item Matched");
            ItemManager.Instance.RemoveItem();
        }
        
    }

    protected void itemMatched()
    {

    }
}
