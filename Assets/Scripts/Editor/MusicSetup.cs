using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class MusicSetup
{
    [MenuItem("Tools/Audio/Setup Music Manager")]
    public static void SetupMusicManager()
    {
        // Create Music folder in Resources if it doesn't exist
        string musicFolderPath = "Assets/Resources/Music";
        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
            AssetDatabase.Refresh();
            Debug.Log("Created Music folder at " + musicFolderPath);
        }

        // Check if MusicManager exists in scene
        MusicManager existingManager = Object.FindObjectOfType<MusicManager>();
        if (existingManager == null)
        {
            // Create MusicManager GameObject
            GameObject musicManagerObj = new GameObject("MusicManager");
            musicManagerObj.AddComponent<MusicManager>();
            Debug.Log("Created MusicManager in the scene");
            
            // Select the new GameObject
            Selection.activeGameObject = musicManagerObj;
            
            // Show helpful message
            EditorUtility.DisplayDialog("Music Manager Setup", 
                "Music Manager has been created.\n\n" +
                "Place your music track in the Resources/Music folder, then drag it to the Music Track field in the Inspector.", 
                "OK");
        }
        else
        {
            Debug.Log("MusicManager already exists in the scene");
            
            // Select the existing GameObject
            Selection.activeGameObject = existingManager.gameObject;
            
            EditorUtility.DisplayDialog("Music Manager Found", 
                "Music Manager already exists in your scene.\n\n" +
                "Place your music track in the Resources/Music folder, then drag it to the Music Track field in the Inspector.", 
                "OK");
        }
    }
}
#endif