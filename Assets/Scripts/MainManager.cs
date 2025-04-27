using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class MainManager : MonoBehaviour
{
    public GameManager game;
    public GameObject gameplayPanel; 
    public GameObject startPanel;
    public GameObject shopPanel;

    public int PlayerMoney = 100;
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
        Debug.Log("Game Started");
        setScene(SceneState.InGame);
        game.StartNewGame();
    }

    public void setScene( SceneState newState)
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
                game.uiManager.OpenShop();

                break;
            case SceneState.Pause:
                break;
            case SceneState.GameOver:
                break;
        }
    }

    public IEnumerator WinRound()
    {
        PlayerMoney += game.Score;
        game.state = GameManager.GameState.RoundWon;
        game.uiManager.UpdateGameplayUI();
        
        yield return new WaitForSeconds(2f);

        setScene(SceneState.Shop);
    }
}
