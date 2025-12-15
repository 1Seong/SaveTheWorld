using DG.Tweening;
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
        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_TVButton);

        isActing = true;
        onMonitor.DOScaleX(0.6f, 0.1f).OnComplete(() =>
        {
            onMonitor.DOScaleY(0.59f, 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                isActing = false;
                if(ChannelOn)
                    channelMonitor.SetActive(true);
            });
        });
    }

    protected override void Off()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_TVButton);

        isActing = true;
        if (ChannelOn)
            channelMonitor.SetActive(false);
        onMonitor.DOScaleY(0.015f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
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
