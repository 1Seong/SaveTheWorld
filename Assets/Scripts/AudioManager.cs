using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum AudioType
{
    BGM_Title, BGM_Ending,

    BGM_TV1, BGM_TV2, BGM_TV3,

    BGM_Syringe, BGM_Crutch, BGM_Jar,
    BGM_Pencil, BGM_Sewing, BGM_Laundry,

    SFX_Etc_ShowBlurr, SFX_Etc_MainButton,
    SFX_Etc_RoomTrans, SFX_Etc_SceneTrans,
    SFX_Etc_InvItemOn, SFX_Etc_InvItemOff,
    SFX_Etc_InvFold, SFX_Etc_TVNext,

    SFX_Note_Letter, SFX_Note_Page, SFX_Note_Cover,

    SFX_Room_Fly, SFX_Room_Door,
    SFX_Room_BottleFill, SFX_Room_BottleDrain,
    SFX_Room_Sink, SFX_Room_Item,
    SFX_Room_Closet, SFX_Room_Light,
    SFX_Room_Window, SFX_Room_Beddings,
    SFX_Room_Curtain, SFX_Room_RemoteButton,
    SFX_Room_TVButton,

    SFX_C_Jump, SFX_C_Land,
    SFX_C_Explode, SFX_C_Hit,

    SFX_L_Die,

    SFX_P_Sharpner, SFX_P_Break, SFX_P_Draw,

    SFX_S_Sewing, SFX_S_PTSD,

    SFX_SY_Fail, SFX_SY_Syringe, SFX_SY_Success,

    SFX_J_Show, SFX_J_End,

    SFX_MG_Type,

    SFX_End_Change, SFX_End_End
}

[System.Serializable]
public class AudioEntry
{
    public AudioType type;
    public AudioClip clip;
    public bool isLoop;
    public bool isBgm;   // true면 BGM, false면 SFX
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;         // BGM 전용
    public AudioSource sfxTriggerSource;  // SFX 트리거 전용
    public AudioSource sfxLoopSource;     // SFX 루프 전용

    [Header("Audio Data")]
    public float BGMFadeTime = 1.0f;
    public float SFXLoopFadeTime = 0.5f;
    public List<AudioEntry> entries;

    private Dictionary<AudioType, AudioEntry> dict;
    private AudioEntry currentBgm;
    private AudioEntry currentSfxLoop;

    void Awake()
    {
        // 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        dict = entries.ToDictionary(e => e.type);
    }

    // ----------- BGM -----------
    public void PlayBgm(AudioType type)
    {
        var entry = dict[type];
        if (!entry.isBgm || !entry.isLoop) return;

        if (currentBgm == entry) return;

        bgmSource.DOKill();
        bgmSource.Stop();
        bgmSource.clip = entry.clip;
        bgmSource.volume = 0f;
        bgmSource.Play();
        bgmSource.DOFade(1f, BGMFadeTime);
        currentBgm = entry;
    }

    public void StopBgm()
    {
        bgmSource.DOKill();
        bgmSource.DOFade(0f, BGMFadeTime)
                 .OnComplete(() => bgmSource.Stop());
        currentBgm = null;
    }

    // ----------- SFX Trigger -----------
    public void PlaySfx(AudioType type)
    {
        var entry = dict[type];
        if (entry.isLoop) return;
        sfxTriggerSource.PlayOneShot(entry.clip);
    }

    // ----------- SFX Loop -----------
    public void LoopSfxOn(AudioType type)
    {
        var entry = dict[type];
        if (entry.isBgm) return;
        if (!entry.isLoop) return;

        if (currentSfxLoop == entry) return;

        sfxLoopSource.DOKill();
        sfxLoopSource.Stop();
        sfxLoopSource.clip = entry.clip;
        sfxLoopSource.volume = 0f;
        sfxLoopSource.Play();
        sfxLoopSource.DOFade(1f, SFXLoopFadeTime);
        currentSfxLoop = entry;
    }

    public void LoopSfxOff()
    {
        sfxLoopSource.DOKill();
        sfxLoopSource.DOFade(0f, SFXLoopFadeTime)
                     .OnComplete(() => sfxLoopSource.Stop());
        currentSfxLoop = null;
    }

    public bool IsLoopSfxPlaying(AudioType type)
    {
        if (currentSfxLoop.type == type) return true;
        else return false;
    }
}
