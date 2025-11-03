using UnityEngine;

public class ItemTarget : MonoBehaviour
{
    [SerializeField] private Item.Items id;

    [Header("# Bottle")]
    [SerializeField] private ItemObject filledBottle;

    public void OnInteract(ItemData data)
    {
        if(id == data.id)
        {
            itemMatched();
            Debug.Log("Item Matched");
            ItemManager.Instance.RemoveItem();
        }
        
    }

    protected virtual void itemMatched()
    {
        switch(id)
        {
            case Item.Items.Bottle:
                if (filledBottle != null)
                {
                    //filledBottle.gameObject.SetActive(true);
                    filledBottle.Collect();
                }
                break;
            case Item.Items.FlyCatcher:

                break;
        }
    }
}
