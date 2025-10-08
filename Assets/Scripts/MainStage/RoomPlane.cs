using UnityEngine;

public class RoomPlane : MonoBehaviour
{
    [SerializeField] private GameObject interactiveParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ActivateInteractives()
    {
        interactiveParent.SetActive(true);
    }

    public void DeactivateInteractives()
    {
        interactiveParent.SetActive(false);
    }
}
