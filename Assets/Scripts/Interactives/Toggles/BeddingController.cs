using UnityEngine;

public class BeddingController : MonoBehaviour
{
    private Bedding[] beddings;

    private void Awake()
    {
        beddings = GetComponentsInChildren<Bedding>();
    }

    public void OnClick()
    {

    }
}
