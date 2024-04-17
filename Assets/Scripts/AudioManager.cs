using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioSource> sfxAudioSources = new List<AudioSource>();
    [SerializeField] private AudioSource voiceLinesAudioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioClip[] voiceLinesClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound("GameplayMusic");
        PlayVoiceLine("Start");
    }

    public void PlaySound(string clipName)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == clipName)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
                break;
            }
        }
    }

    public void PlaySFX(string clipName)
    {
        foreach (AudioClip clip in sfxClips)
        {
            if (clip.name == clipName)
            {
                AudioSource freeSource = GetFreeSFXAudioSource();
                if (freeSource != null)
                {
                    freeSource.clip = clip;
                    freeSource.Play();
                }
                break;
            }
        }
    }

    public void PlayVoiceLine(string clipName)
    {
        foreach (AudioClip clip in voiceLinesClips)
        {
            if (clip.name == clipName)
            {
                voiceLinesAudioSource.clip = clip;
                voiceLinesAudioSource.Play();
                break;
            }
        }
    }

    private AudioSource GetFreeSFXAudioSource()
    {
        foreach (AudioSource source in sfxAudioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }


}
