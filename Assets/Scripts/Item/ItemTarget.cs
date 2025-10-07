using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTarget : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Item.Items id;

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
