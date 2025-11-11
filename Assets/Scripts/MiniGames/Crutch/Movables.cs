using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class Movables : MonoBehaviour
{
    public float gameDuration = 30f;
    public float initUpdateTime = 0.5f;
    public float finalUpdateTime = 0.1f;
    public float slowDownStep = 0.2f;
    public float moveDis = 0.1f;
    public float spawnRate = 0.4f;
    public Player player;
    public Spawner spawner;

    float updateTime;
    float currentTime = 0f;
    float totalTime = 0f;

    private void Start()
    {
        updateTime = initUpdateTime;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        totalTime += Time.deltaTime;
        currentTime += Time.deltaTime;

        float t = Mathf.InverseLerp(0f, gameDuration, totalTime);
        updateTime = Mathf.Lerp(initUpdateTime, finalUpdateTime, t);

        if(currentTime >= updateTime)
        {
            currentTime = 0f;

            float r = Random.Range(0f, 1f);
            if(r < spawnRate)
                spawner.Spawn();
            transform.Translate(-moveDis, 0f, 0f);
        }
        if(totalTime >= gameDuration)
        {
            MiniGameManager.instance.IsPlaying = false;
            player.Die();
        }
    }
}
