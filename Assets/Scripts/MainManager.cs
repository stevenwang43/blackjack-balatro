using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameManager game;
    public GameObject gameplayPanel; 
    public GameObject startPanel;
    public GameObject shopPanel;
    
    // Reference to music manager
    private MusicManager musicManager;
    
    public enum SceneState
    {
        StartMenu,
        InGame,
        Shop,
        Pause,
        GameOver
    }
    
    SceneState state = SceneState.StartMenu;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get or create the music manager
        musicManager = FindOrCreateMusicManager();
        
        setScene(SceneState.StartMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        game.StartNewGame();
        setScene(SceneState.InGame);
    }

    public void setScene(SceneState newState)
    {
        state = newState;
        switch (state)
        {
            case SceneState.StartMenu:
                startPanel.SetActive(true);
                gameplayPanel.SetActive(false);
                shopPanel.SetActive(false);
                break;
            case SceneState.InGame:
                startPanel.SetActive(false);
                gameplayPanel.SetActive(true);
                shopPanel.SetActive(false);
                break;
            case SceneState.Shop:
                startPanel.SetActive(false);
                gameplayPanel.SetActive(false);
                shopPanel.SetActive(true);
                break;
            case SceneState.Pause:
                // If we implement a pause feature, we could pause the music here
                // if (musicManager != null) musicManager.PauseMusic();
                break;
            case SceneState.GameOver:
                break;
        }
    }
    
    // Find existing MusicManager or create one if it doesn't exist
    private MusicManager FindOrCreateMusicManager()
    {
        MusicManager manager = FindObjectOfType<MusicManager>();
        if (manager == null)
        {
            GameObject musicObj = new GameObject("MusicManager");
            manager = musicObj.AddComponent<MusicManager>();
            Debug.Log("Created MusicManager for background music");
        }
        return manager;
    }
}
