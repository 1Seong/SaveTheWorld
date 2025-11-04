using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f), 0.2f).SetLoops(4);
                transform.DOMoveY(transform.position.y - 5f, 1f).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
                break;
        }
    }
}
