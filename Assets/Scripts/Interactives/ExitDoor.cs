using UnityEngine;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    private Button doorButton;

    private void Awake()
    {
        doorButton = GetComponent<Button>();
    }

    private void Start()
    {
        GameManager.GameAllClearedEvent += ActivateDoor;
    }

    private void OnDisable()
    {
        GameManager.GameAllClearedEvent -= ActivateDoor;
    }

    private void ActivateDoor()
    {
        doorButton.interactable = true;
    }
}
