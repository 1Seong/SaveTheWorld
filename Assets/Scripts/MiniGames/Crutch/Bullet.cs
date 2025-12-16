using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Disappear), 8f);
    }

    private void Update()
    {
        if(!MiniGameManager.instance.IsPlaying)
            Disappear();
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
