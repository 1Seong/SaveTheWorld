using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] bullets;
    public Transform movables;
    public Transform center;

    public void Spawn()
    {
        int i = Random.Range(0, 3);
        var bullet = Instantiate(bullets[i], center);
        bullet.transform.parent = movables;
    }
}
