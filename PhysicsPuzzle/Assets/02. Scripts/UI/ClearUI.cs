using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private TextMeshProUGUI bestClearTimeText;
    [SerializeField] private Button lobbyButton;
    
    protected override UIState GetUIState()
    {
        return UIState.Clear;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        
        lobbyButton.onClick.AddListener(OnClickLobbyButton);
    }

    private void OnClickLobbyButton()
    {
        UIManager.Instance.OnClickStart();
    }
}
