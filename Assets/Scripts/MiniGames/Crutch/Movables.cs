using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class Movables : MonoBehaviour
{
    public float gameDuration = 30f;
    public float slowDownRate = 0.2f;
    public Vector3 velocity = new Vector3(0.2f, 0f, 0f);
    public Vector3 acceleration = new Vector3(0.1f, 0f, 0f);
    public float spawnTime = 4f;
    public float spawnIncreaseRate = 0.1f;
    public Player player;
    public Spawner spawner;

    float totalTime = 0f;
    float currentTime = 0f;

    private void Update()
    {
        if(!GameManager.Instance.IsPlaying) return;

        if (MiniGameManager.instance.IsPlaying)
        {
            totalTime += Time.deltaTime;
            currentTime += Time.deltaTime;

            velocity += acceleration * Time.deltaTime;
            spawnTime -= spawnIncreaseRate * Time.deltaTime;
        }

        transform.position -= velocity * Time.deltaTime;

        if(currentTime > spawnTime)
        {
            currentTime = 0f;
            spawner.Spawn();
        }

        if (totalTime >= gameDuration)
        {
            MiniGameManager.instance.IsPlaying = false;
            player.Die();
        }
    }

    public void SlowDown()
    {
        velocity -= slowDownRate * velocity;
    }
}
