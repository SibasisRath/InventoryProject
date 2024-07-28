using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService
{
    private readonly SoundSO soundScriptableObject;
    private readonly AudioSource audioEffects;

    private EventService eventService;

    public SoundService(SoundSO soundScriptableObject, AudioSource audioEffectSource)
    {
        this.soundScriptableObject = soundScriptableObject;
        audioEffects = audioEffectSource;
    }

    public void Init(EventService eventService) 
    {
        this.eventService = eventService;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        eventService.OnButtonSelected.AddListener(PlaySoundEffects);
    }

    private void UnsubscribeToEvents()
    {
        eventService.OnButtonSelected.RemoveListener(PlaySoundEffects);
    }

    public void PlaySoundEffects(SoundTypes soundType)
    {
        bool loopSound = false;
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            audioEffects.loop = loopSound;
            audioEffects.clip = clip;
            audioEffects.PlayOneShot(clip);
        }
        else
            Debug.LogError("No Audio Clip selected.");
    }

    private AudioClip GetSoundClip(SoundTypes soundType)
    {
        Sounds sound = Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
        if (sound.audio != null)
            return sound.audio;
        return null;
    }

    ~SoundService() 
    {
        UnsubscribeToEvents();
    }
}
