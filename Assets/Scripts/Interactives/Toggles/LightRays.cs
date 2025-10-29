using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class LightRays : MonoBehaviour
{
    private MeshRenderer[] rays;

    private void Awake()
    {
        rays = GetComponentsInChildren<MeshRenderer>();

        foreach (var m in rays)
        {
            m.material.DOFade(0f, 0f);
        }
    }

    public void LightAppear()
    {
        foreach (var m in rays)
        {
            m.material.DOFade(0.25f, 0.3f);
        }
    }

    public void LightDisappear()
    {
        foreach (var m in rays)
        {
            m.material.DOFade(0f, 0.3f);
        }
    }
       
}
