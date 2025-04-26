using UnityEngine;

public class RunManager : MonoBehaviour
{
    public GameManager game;
    public UIManager ui;
    enum RunState
    {
        StartMenu,
        InGame,
        Shop,
        Pause,
        GameOver
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        game.StartNewGame();
        ui.UpdateUI();
    }
}
