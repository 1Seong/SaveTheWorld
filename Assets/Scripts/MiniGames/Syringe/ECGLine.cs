using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class ECGLine : MonoBehaviour
{
    public int points = 256;
    public float width = 0.02f;
    public float amplitude = 0.5f;
    public float frequency = 5f;
    public float speed = 2f;

    LineRenderer lr;
    float offset = 0;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = points;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.useWorldSpace = false;
    }

    void Update()
    {
        offset += Time.deltaTime * speed;
        for (int i = 0; i < points; i++)
        {
            float x = (float)i / points * 10f;
            float y = Mathf.Sin((x + offset) * frequency) * amplitude;
            lr.SetPosition(i, new Vector3(x - 5f, y, 0)); // Áß¾Ó Á¤·Ä
        }
    }
}

