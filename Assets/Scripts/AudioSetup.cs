using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup Audio Manager")]
    public static void SetupAudioManager()
    {
        // Create Resources/Sounds directory if it doesn't exist
        string folderPath = "Assets/Resources/Sounds";
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
            Debug.Log("Created Sounds directory at " + folderPath);
        }

        // Check if AudioManager already exists in the scene
        AudioManager existingManager = GameObject.FindObjectOfType<AudioManager>();
        if (existingManager == null)
        {
            // Create new AudioManager GameObject
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.AddComponent<AudioManager>();
            Debug.Log("Created AudioManager GameObject in the scene");
        }
        else
        {
            Debug.Log("AudioManager already exists in the scene");
        }
    }
#endif
}