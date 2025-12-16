using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_C_Bullet);

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
