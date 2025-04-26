using UnityEngine;
using UnityEngine.UIElements;

public class MainManager : MonoBehaviour
{
    public GameManager game;
    public GameObject gameplayPanel; 
    public GameObject startPanel;
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

    public void setScene( SceneState newState)
    {
        state = newState;
        switch (state)
        {
            case SceneState.StartMenu:
                startPanel.SetActive(true);
                gameplayPanel.SetActive(false);
                break;
            case SceneState.InGame:
                startPanel.SetActive(false);
                gameplayPanel.SetActive(true);
                break;
            case SceneState.Shop:
                break;
            case SceneState.Pause:
                break;
            case SceneState.GameOver:
                break;
        }
    }
}
