using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static AudioManager Instance { get; private set; }

    // Sound effect categories
    public enum SoundType
    {
        CardDeal,
        CardFlip,
        CardShuffle,
        Win,
        Lose,
        ButtonClick,
        MoneyGain,
        Denied
    }

    // Serialized dictionary to assign sounds in the Inspector
    [System.Serializable]
    public class SoundEffect
    {
        public SoundType type;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1.0f;
        [Range(0.5f, 1.5f)]
        public float pitch = 1.0f;
        [Range(0f, 0.5f)]
        public float pitchVariation = 0.1f;
    }

    // List of sound effects
    [Tooltip("Manually assign sound effects here. These will override any auto-assigned sounds.")]
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    // List of all available audio clips for manual assignment
    [Tooltip("All available audio clips detected in the Resources/Sounds folder")]
    [SerializeField] private List<AudioClip> availableClips = new List<AudioClip>();

    // AudioSources for playing sounds
    private AudioSource mainAudioSource;
    private List<AudioSource> pooledAudioSources = new List<AudioSource>();
    private Dictionary<SoundType, SoundEffect> soundDictionary = new Dictionary<SoundType, SoundEffect>();

    // Flag to indicate whether we should use automatic assignment based on names
    [Tooltip("If true, sounds will be auto-categorized by filename. If false, only manually assigned sounds will be used.")]
    public bool useAutomaticAssignment = true;

    private void Awake()
    {
        // Singleton pattern setup
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
        // Create main audio source
        mainAudioSource = gameObject.AddComponent<AudioSource>();

        // Create a dictionary for faster lookup
        CreateSoundDictionary();

        // Create a few pooled audio sources for overlapping sounds
        for (int i = 0; i < 5; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            pooledAudioSources.Add(source);
        }
    }

    // Create the sound dictionary from the sound effects list
    private void CreateSoundDictionary()
    {
        soundDictionary.Clear();
        
        // First, add manually assigned sounds
        foreach (var sound in soundEffects)
        {
            if (sound.clip != null)
            {
                soundDictionary[sound.type] = sound;
            }
        }
        
        // Then, if auto-assignment is enabled, add automatically detected sounds
        // but don't override manual assignments
        if (useAutomaticAssignment)
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
            foreach (AudioClip clip in clips)
            {
                if (TryParseClipName(clip.name, out SoundType type))
                {
                    // Only add if not already manually assigned
                    if (!soundDictionary.ContainsKey(type))
                    {
                        SoundEffect effect = new SoundEffect
                        {
                            type = type,
                            clip = clip,
                            volume = 1.0f,
                            pitch = 1.0f,
                            pitchVariation = 0.1f
                        };
                        
                        soundDictionary[type] = effect;
                    }
                }
            }
        }
    }

    // Play a sound from the dictionary
    public void PlaySound(SoundType type)
    {
        if (soundDictionary.TryGetValue(type, out SoundEffect sound))
        {
            PlaySound(sound);
        }
        else
        {
            Debug.LogWarning($"Sound of type {type} not found in the AudioManager.");
        }
    }

    // Play a specific sound effect
    private void PlaySound(SoundEffect sound)
    {
        // Don't try to play null clips
        if (sound.clip == null)
            return;

        // Find an available audio source
        AudioSource source = GetAvailableAudioSource();
        
        // Set up the audio source
        source.clip = sound.clip;
        source.volume = sound.volume;
        
        // Apply random pitch variation
        float randomPitch = sound.pitch + Random.Range(-sound.pitchVariation, sound.pitchVariation);
        source.pitch = randomPitch;
        
        // Play the sound
        source.Play();
    }

    // Find an audio source that's not currently playing
    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in pooledAudioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        
        // If all sources are busy, use the main one
        return mainAudioSource;
    }

    // Load sound effects from Resources folder
    public void LoadSoundEffects()
    {
        // Load all audio clips from the Resources/Sounds folder
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
        
        // Update available clips list
        availableClips.Clear();
        if (clips != null)
        {
            availableClips.AddRange(clips);
        }
        
        // Recreate the sound dictionary
        CreateSoundDictionary();
        
        Debug.Log($"Loaded {clips.Length} sound clips from Resources/Sounds");
    }

    // Try to match clip name with sound types
    private bool TryParseClipName(string clipName, out SoundType type)
    {
        clipName = clipName.ToLower();
        
        if (clipName.Contains("deal") || clipName.Contains("draw"))
            type = SoundType.CardDeal;
        else if (clipName.Contains("flip") || clipName.Contains("turn"))
            type = SoundType.CardFlip;
        else if (clipName.Contains("shuffle"))
            type = SoundType.CardShuffle;
        else if (clipName.Contains("win"))
            type = SoundType.Win;
        else if (clipName.Contains("lose") || clipName.Contains("bust"))
            type = SoundType.Lose;
        else if (clipName.Contains("click") || clipName.Contains("button"))
            type = SoundType.ButtonClick;
        else if (clipName.Contains("money") || clipName.Contains("coin"))
            type = SoundType.MoneyGain;
        else if (clipName.Contains("card"))
            type = SoundType.CardFlip; // Default for general card sounds
        else
        {
            type = SoundType.CardDeal; // Default
            return false;
        }
        
        return true;
    }

#if UNITY_EDITOR
    // Additional method to help with sound assignment in the editor
    public void RefreshSoundList()
    {
        LoadSoundEffects();
        
        // Keep any existing manual assignments
        List<SoundEffect> newEffects = new List<SoundEffect>();
        
        // Add all existing manual assignments that have clips
        foreach (var effect in soundEffects)
        {
            if (effect.clip != null)
            {
                newEffects.Add(effect);
            }
        }
        
        soundEffects = newEffects;
    }
#endif
}