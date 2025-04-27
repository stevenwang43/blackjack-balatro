using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class AudioFileProcessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // Check if any audio files were imported to the Sounds folder
        bool soundsImported = false;
        foreach (string asset in importedAssets)
        {
            if (asset.EndsWith(".wav") && asset.Contains("/Sounds/"))
            {
                soundsImported = true;
                Debug.Log("Detected new sound file: " + asset);
            }
        }

        // If sounds were imported, trigger a reload in AudioManager
        if (soundsImported)
        {
            AudioManager[] managers = Object.FindObjectsOfType<AudioManager>();
            foreach (var manager in managers)
            {
                Debug.Log("Reloading sound files in AudioManager");
                manager.LoadSoundEffects();
            }
        }
    }
}
#endif