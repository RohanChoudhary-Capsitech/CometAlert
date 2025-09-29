using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioManager : MonoBehaviour
{
    [Header("Sound Collections")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    private Dictionary<string, Sound> musicDict;
    private Dictionary<string, Sound> sfxDict;
    public static AudioManager Instance { get; private set; }

    public bool musicIsInMute { get; private set; }
    public bool sfxIsInMute { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Corrected: destroy the GameObject, not just 'this'
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        musicDict = new Dictionary<string, Sound>();
        sfxDict = new Dictionary<string, Sound>();

        musicIsInMute = false;
        sfxIsInMute = false; // Corrected: SFX should not be muted by default

        foreach (var sound in musicSounds)
        {
            if (!musicDict.ContainsKey(sound.name))
                musicDict.Add(sound.name, sound);
        }
        foreach (var sound in sfxSounds)
        {
            if (!sfxDict.ContainsKey(sound.name))
                sfxDict.Add(sound.name, sound);
        }
    }

    #region Music Methods

    public void PlayMusic(string name, bool loop = false)
    {
        if (!musicDict.TryGetValue(name, out Sound s) || s.clip == null)
        {
            Debug.LogWarning($"Music sound not found or clip missing: {name}");
            return;
        }

        AudioSource src = CreateAudioSource();
        src.clip = s.clip;
        src.loop = loop;
        src.volume = s.volume;
        src.mute = musicIsInMute;
        src.Play();

        if (!loop)
            StartCoroutine(DelayCallback(src.clip.length, () => Destroy(src.gameObject)));
    }

    public void StopMusic(string name)
    {
        if (!musicDict.TryGetValue(name, out Sound s) || s.clip == null)
        {
            Debug.LogWarning($"Music sound not found or clip missing: {name}");
            return;
        }

        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (src.clip == s.clip && src.isPlaying)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
        }
    }

    public void MuteMusic(bool value)
    {
        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (Array.Exists(musicSounds, s => s.clip == src.clip))
                src.mute = value;
        }
        musicIsInMute = value;
    }

    #endregion

    #region SFX Methods

    public void PlaySfx(string name, bool loop = false)
    {
        if (!sfxDict.TryGetValue(name, out Sound s) || s.clip == null)
        {
            Debug.LogWarning($"SFX sound not found or clip missing: {name}");
            return;
        }

        AudioSource src = CreateAudioSource();
        src.clip = s.clip;
        src.loop = loop;
        src.volume = s.volume;
        src.mute = sfxIsInMute;
        src.Play();

        if (!loop)
            StartCoroutine(DelayCallback(src.clip.length, () => Destroy(src.gameObject)));
    }

    public void StopSfx(string name)
    {
        if (!sfxDict.TryGetValue(name, out Sound s) || s.clip == null)
        {
            Debug.LogWarning($"SFX sound not found or clip missing: {name}");
            return;
        }

        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (src.clip == s.clip && src.isPlaying)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
        }
    }

    public void MuteSfx(bool value)
    {
        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (Array.Exists(sfxSounds, s => s.clip == src.clip))
                src.mute = value;
        }
        sfxIsInMute = value;
    }

    #endregion

    #region Global Controls

    public void PauseAll()
    {
        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            src.Pause();
    }

    public void ResumeAll()
    {
        foreach (var src in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            src.UnPause();
    }

    #endregion

    #region Helpers

    private AudioSource CreateAudioSource()
    {
        GameObject go = new GameObject("AudioSource_" + Guid.NewGuid());
        go.transform.SetParent(this.transform);
        return go.AddComponent<AudioSource>();
    }

    private IEnumerator DelayCallback(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    #endregion
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    // Removed unused 'mute' field
}