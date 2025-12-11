using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemTarget : MonoBehaviour
{
    [SerializeField] private Item.Items id;

    [Header("# Bottle")]
    [SerializeField] private ItemObject filledBottle;

    [Header("# Fly")]
    [SerializeField] private Button letter;

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
                ItemManager.Instance.isFlyCatched = true;
                transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f), 0.4f).SetLoops(4);
                GetComponent<SpriteRenderer>().DOFade(0f, 1.6f);
                transform.DOMoveY(transform.position.y - 3f, 1.6f).OnComplete(() =>
                {
                    letter.interactable = true;
                    Destroy(gameObject);
                });
                break;
        }
    }

    public void ApplyFlyData()
    {
        if (id != Item.Items.FlyCatcher) return;

        letter.interactable = true;
        Destroy(gameObject);
    }
}
