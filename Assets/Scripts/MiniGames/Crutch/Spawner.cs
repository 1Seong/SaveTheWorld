using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] bullets;
    public Transform movables;

    public void Spawn()
    {
        int i = Random.Range(0, 3);
        Instantiate(bullets[i], movables);
    }
}
