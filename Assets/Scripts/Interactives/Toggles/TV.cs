using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TV : ToggleInteractives
{
    [SerializeField] Transform onMonitor;
    [SerializeField] GameObject channelMonitor;

    [SerializeField] bool _channelOn = false;


    public bool ChannelOn
    {
        get => _channelOn;
        set
        {
            if(!_channelOn && value && IsActive)
            {
                channelMonitor.SetActive(true);
            }
            _channelOn = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void On()
    {
        isActing = true;
        onMonitor.DOScaleX(0.75f, 0.1f).OnComplete(() =>
        {
            onMonitor.DOScaleY(0.73f, 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                isActing = false;
                if(ChannelOn)
                    channelMonitor.SetActive(true);
            });
        });
    }

    protected override void Off()
    {
        isActing = true;
        if (ChannelOn)
            channelMonitor.SetActive(false);
        onMonitor.DOScaleY(0.02f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            onMonitor.DOScaleX(0f, 0.1f).OnComplete(() =>
            {
                isActing = false;
            });
        });
    }

    public void channelOnCallBack()
    {
        ChannelOn = true;
    }

}
