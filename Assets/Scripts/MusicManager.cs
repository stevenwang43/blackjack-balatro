using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton pattern
    public static MusicManager Instance { get; private set; }
    
    // Audio source for music playback
    private AudioSource musicSource;
    
    [Header("Music Settings")]
    [Tooltip("Background music track to play")]
    public AudioClip musicTrack;
    
    [Range(0f, 1f)]
    [Tooltip("Music volume")]
    public float musicVolume = 0.5f;
    
    [Tooltip("Should music loop?")]
    public bool loopMusic = true;
    
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Initialize()
    {
        // Create audio source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = loopMusic;
        musicSource.volume = musicVolume;
        
        // Start playing if we have a track assigned
        if (musicTrack != null)
        {
            PlayMusic();
        }
    }
    
    /// <summary>
    /// Play the assigned music track
    /// </summary>
    public void PlayMusic()
    {
        if (musicTrack == null)
            return;
            
        musicSource.clip = musicTrack;
        musicSource.Play();
    }
    
    /// <summary>
    /// Play a specific music track
    /// </summary>
    public void PlayMusic(AudioClip music)
    {
        if (music == null)
            return;
            
        musicTrack = music;
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }
    
    /// <summary>
    /// Stop music playback
    /// </summary>
    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
    
    /// <summary>
    /// Pause music playback
    /// </summary>
    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }
    
    /// <summary>
    /// Resume paused music
    /// </summary>
    public void ResumeMusic()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
            musicSource.UnPause();
    }
    
    /// <summary>
    /// Set music volume (0-1)
    /// </summary>
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }
    
    /// <summary>
    /// Fade music in over time
    /// </summary>
    public void FadeIn(float duration = 1.0f)
    {
        if (musicSource.clip == null)
            return;
            
        StopAllCoroutines();
        musicSource.volume = 0;
        
        if (!musicSource.isPlaying)
            musicSource.Play();
            
        StartCoroutine(FadeVolumeCoroutine(0, musicVolume, duration));
    }
    
    /// <summary>
    /// Fade music out over time
    /// </summary>
    public void FadeOut(float duration = 1.0f)
    {
        if (!musicSource.isPlaying)
            return;
            
        StopAllCoroutines();
        StartCoroutine(FadeVolumeCoroutine(musicSource.volume, 0, duration));
    }
    
    private System.Collections.IEnumerator FadeVolumeCoroutine(float startVolume, float targetVolume, float duration)
    {
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        musicSource.volume = targetVolume;
        
        if (targetVolume == 0)
            musicSource.Stop();
    }
}