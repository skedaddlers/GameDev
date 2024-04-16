using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private void Awake()
    {
        if(Instance == null)
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
    }

    public void PlaySound(string clipName)
    {
        foreach(AudioClip clip in audioClips)
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
}
