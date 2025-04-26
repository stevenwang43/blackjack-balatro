using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button drawCardButton;
    public DeckManager deckManager;
    public HandManager handManager;

    void Start()
    {
        drawCardButton.onClick.AddListener(() => deckManager.DrawCard(handManager));
    }
}
