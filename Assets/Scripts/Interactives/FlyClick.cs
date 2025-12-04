using UnityEngine;

public class FlyClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<Animator>().SetTrigger("Click");
    }
}
