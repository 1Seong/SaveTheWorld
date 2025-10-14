using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public static event Action<ItemData, Vector3> CollectEvent;

    private void Start()
    {
        if (Item.IsCollected(data.id)) Destroy(gameObject);

        //GameManager.Instance.PhaseChangedEvent += Unlock;
    }

    public void Collect()
    {
        if (ItemManager.Instance.IsHolding) return;

        Item.Collect(data.id);
        CollectEvent?.Invoke(data, transform.position);
        Destroy(gameObject);
    }

    /*
    public void Unlock()
    {
        if (data.unlockPhase != GameManager.Instance.CurrentPhase) return;

        GetComponent<Button>().interactable = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    */
}
