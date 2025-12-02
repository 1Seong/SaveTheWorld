using UnityEngine;

public class RoomInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager.Instance.LoadAll();
        TutorialManager.Instance.StartTutorial();
    }
}
