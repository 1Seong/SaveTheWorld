
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;

    [SerializeField] float masterVol = 0f, bgmVol = 0f, sfxVol = 0f;
    public bool masterMute = false, bgmMute = false, sfxMute = false;

    private void Start()
    {
        m_AudioMixer.SetFloat("Master", 0f);
        m_AudioMixer.SetFloat("BGM", 0f);
        m_AudioMixer.SetFloat("SFX", 0f);
    }

    public void SetMasterVolume(float volume)
    {
        masterVol = Mathf.Log10(volume) * 20;
        m_AudioMixer.SetFloat("Master", masterVol);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVol = Mathf.Log10(volume) * 20;
        m_AudioMixer.SetFloat("BGM", bgmVol);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVol = Mathf.Log10(volume) * 20;
        m_AudioMixer.SetFloat("SFX", sfxVol);
    }

    public void MuteMasterVolume()
    {
        if (masterMute) // mute -> normal
        {
            masterMute = false;

            m_AudioMixer.SetFloat("Master", masterVol);
        }
        else // normal -> mute
        {
            masterMute = true;

            m_AudioMixer.SetFloat("Master", -80f);
        }
    }

    public void MuteBGM()
    {
        if (bgmMute) // mute -> normal
        {
            bgmMute = false;

            m_AudioMixer.SetFloat("BGM", bgmVol);
        }
        else // normal -> mute
        {
            bgmMute = true;

            m_AudioMixer.SetFloat("BGM", -80f);
        }
    }

    public void MuteSFX()
    {
        if (sfxMute) // mute -> normal
        {
            sfxMute = false;

            m_AudioMixer.SetFloat("SFX", sfxVol);
        }
        else // normal -> mute
        {
            sfxMute = true;

            m_AudioMixer.SetFloat("SFX", -80f);
        }
    }

}
