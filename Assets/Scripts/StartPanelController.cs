using UnityEngine;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    public Button startButton;

    public void SetupStartButton(System.Action onStart)
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onStart());
    }
}
