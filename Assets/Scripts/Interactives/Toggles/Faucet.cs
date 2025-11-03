using DG.Tweening;
using UnityEngine;

public class Faucet : ToggleInteractives
{
    [SerializeField] ParticleSystem waterParticleObj;
    [SerializeField] GameObject waterTargetObj;

    protected override void On()
    {
        isActing = true;
        waterParticleObj.Play();
        waterTargetObj.SetActive(true);
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, -90f, 0f), 0.15f).SetEase(Ease.OutExpo).SetLoops(3).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        waterParticleObj.Stop();
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f), 0.15f).SetEase(Ease.OutExpo).SetLoops(3).OnComplete(() =>
        {
            waterTargetObj.SetActive(false);
            isActing = false;
        });
    }
}
