using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    // Reference to the AudioManager being edited
    private AudioManager audioManager;

    // Dictionary to track fold-out states for each sound type
    private Dictionary<AudioManager.SoundType, bool> foldoutStates = new Dictionary<AudioManager.SoundType, bool>();

    private void OnEnable()
    {
        audioManager = (AudioManager)target;
        
        // Initialize all foldout states to false
        foreach (AudioManager.SoundType soundType in System.Enum.GetValues(typeof(AudioManager.SoundType)))
        {
            foldoutStates[soundType] = false;
        }
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector for basic properties
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound Effect Assignment", EditorStyles.boldLabel);

        // Button to refresh sound list
        if (GUILayout.Button("Refresh Available Sounds"))
        {
            audioManager.RefreshSoundList();
        }

        EditorGUILayout.Space();
        
        // Draw sound assignment section
        EditorGUILayout.LabelField("Manual Sound Type Assignment", EditorStyles.boldLabel);
        
        // Get all available audio clips
        List<AudioClip> availableClips = GetAvailableAudioClips();
        
        // No sounds found message
        if (availableClips.Count == 0)
        {
            EditorGUILayout.HelpBox("No sound files found in Resources/Sounds folder.\nAdd .wav files to that folder to get started.", MessageType.Info);
            return;
        }
        
        // Toggle for using automatic assignment
        audioManager.useAutomaticAssignment = EditorGUILayout.Toggle(new GUIContent("Use Automatic Assignment", 
            "When enabled, sounds will be automatically categorized by filename, but manual assignments take priority."), 
            audioManager.useAutomaticAssignment);
        
        EditorGUILayout.Space();
        
        // Display each sound type as a foldout with assignment dropdowns
        foreach (AudioManager.SoundType soundType in System.Enum.GetValues(typeof(AudioManager.SoundType)))
        {
            // Create the foldout for this sound type
            foldoutStates[soundType] = EditorGUILayout.Foldout(foldoutStates[soundType], soundType.ToString());
            
            if (foldoutStates[soundType])
            {
                EditorGUI.indentLevel++;
                
                // Find the current assigned sound for this type
                AudioManager.SoundEffect currentEffect = null;
                int currentEffectIndex = -1;
                
                for (int i = 0; i < audioManager.soundEffects.Count; i++)
                {
                    if (audioManager.soundEffects[i].type == soundType && audioManager.soundEffects[i].clip != null)
                    {
                        currentEffect = audioManager.soundEffects[i];
                        currentEffectIndex = i;
                        break;
                    }
                }
                
                // Create a list of clip names including "None" as the first option
                List<string> clipNames = new List<string> { "None" };
                foreach (AudioClip clip in availableClips)
                {
                    clipNames.Add(clip.name);
                }
                
                // Find the index in the dropdown list
                int currentIndex = 0;
                if (currentEffect != null)
                {
                    // Find the index of the current clip in availableClips
                    for (int i = 0; i < availableClips.Count; i++)
                    {
                        if (availableClips[i] == currentEffect.clip)
                        {
                            currentIndex = i + 1; // +1 because "None" is at index 0
                            break;
                        }
                    }
                }
                
                // Display the dropdown
                int newIndex = EditorGUILayout.Popup("Sound Clip", currentIndex, clipNames.ToArray());
                
                // If the selection changed
                if (newIndex != currentIndex)
                {
                    if (newIndex == 0) // None selected
                    {
                        // If there was a previous assignment, remove it
                        if (currentEffectIndex >= 0)
                        {
                            audioManager.soundEffects.RemoveAt(currentEffectIndex);
                        }
                    }
                    else
                    {
                        AudioClip selectedClip = availableClips[newIndex - 1]; // -1 because "None" is at index 0
                        
                        // If there was an existing effect, update it
                        if (currentEffectIndex >= 0)
                        {
                            audioManager.soundEffects[currentEffectIndex].clip = selectedClip;
                        }
                        // Otherwise create a new effect
                        else
                        {
                            AudioManager.SoundEffect newEffect = new AudioManager.SoundEffect
                            {
                                type = soundType,
                                clip = selectedClip,
                                volume = 1.0f,
                                pitch = 1.0f,
                                pitchVariation = 0.1f
                            };
                            
                            audioManager.soundEffects.Add(newEffect);
                        }
                    }
                    
                    // Mark the object as dirty to ensure saving
                    EditorUtility.SetDirty(audioManager);
                }
                
                // If a sound is assigned, show volume and pitch controls
                if (currentEffect != null)
                {
                    currentEffect.volume = EditorGUILayout.Slider("Volume", currentEffect.volume, 0.0f, 1.0f);
                    currentEffect.pitch = EditorGUILayout.Slider("Pitch", currentEffect.pitch, 0.5f, 1.5f);
                    currentEffect.pitchVariation = EditorGUILayout.Slider("Pitch Variation", currentEffect.pitchVariation, 0.0f, 0.5f);
                    
                    // Add a button to test the sound
                    if (GUILayout.Button("Test Sound"))
                    {
                        // Play the sound in editor mode - this creates an AudioSource for preview
                        if (currentEffect.clip != null)
                        {
                            AudioSource.PlayClipAtPoint(currentEffect.clip, Vector3.zero, currentEffect.volume);
                        }
                    }
                }
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
    }

    // Get all available audio clips from Resources/Sounds folder
    private List<AudioClip> GetAvailableAudioClips()
    {
        List<AudioClip> clips = new List<AudioClip>();
        
        // Use SerializedObject to access serialized properties
        SerializedObject serializedManager = new SerializedObject(audioManager);
        SerializedProperty availableClipsProperty = serializedManager.FindProperty("availableClips");
        
        if (availableClipsProperty != null && availableClipsProperty.isArray)
        {
            for (int i = 0; i < availableClipsProperty.arraySize; i++)
            {
                SerializedProperty clipProperty = availableClipsProperty.GetArrayElementAtIndex(i);
                AudioClip clip = clipProperty.objectReferenceValue as AudioClip;
                
                if (clip != null)
                {
                    clips.Add(clip);
                }
            }
        }
        
        return clips;
    }
}