using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private AudioListener _audioListener;

    private readonly Dictionary<string, AudioClip> _sfxClips = new();
    private readonly Dictionary<string, AudioClip> _musicClips = new();

    protected override void Awake()
    {
        base.Awake();

        CreateAudioListener();
        CreateAudioSources();

        LoadClips();
    }

    private void CreateAudioSources()
    {
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void CreateAudioListener()
    {
        if(_audioListener == null)
        {
            _audioListener = gameObject.AddComponent<AudioListener>();
        }
    }

    private void LoadClips()
    {
        AudioClip[] sfx = Resources.LoadAll<AudioClip>("SFX");
        AudioClip[] music = Resources.LoadAll<AudioClip>("Music");

        foreach (var clip in sfx)
        {
            _sfxClips[clip.name] = clip;
        }

        foreach (var clip in music)
        {
            _musicClips[clip.name] = clip;
        }

        Log.Info($"Loaded {_sfxClips.Count} SFX and {_musicClips.Count} music clips.");
    }

    public void PlaySFX(string name, float pitch = 1f)
    {
        if (_sfxClips.TryGetValue(name, out var clip))
        {
            _sfxSource.pitch = pitch;
            _sfxSource.PlayOneShot(clip);
        }
        else
        {
            Log.Warn($"SFX '{name}' not found in Resources/SFX");
        }
    }

    public void PlayMusic(string name, bool loop = true)
    {
        if (_musicClips.TryGetValue(name, out var clip))
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
        else
        {
            Log.Warn($"Music '{name}' not found in Resources/Music");
        }
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void ToggleSFX(bool value)
    {
        _sfxSource.mute = value;
    }
    public void ToggleMusic(bool value)
    {
        _musicSource.mute = value;
    }
    public void SetMusicVolume(float volume) => _musicSource.volume = volume;
    public void SetSFXVolume(float volume) => _sfxSource.volume = volume;
}
