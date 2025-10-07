using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void ConvertViewLeft()
    {
        ConvertView(90f);
    }

    private void ConvertViewRight()
    {
        ConvertView(-90f);
    }

    private void ConvertView(float targetRot)
    {

    }
}
