using UnityEngine;
using UnityEngine.Rendering;

public class SilkEffect : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform needle;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, needle.localPosition);
    }
}
