using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private TextMeshProUGUI bestClearTimeText;
    [SerializeField] private Button lobbyButton;

    private float bestScore;
    private string formattedTime;
    private SCENE_TYPE sceneType;
    
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
    
    // float 값 분:초로 변환
    private string TimeFormatting(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        formattedTime = $"{minutes:00}분 {seconds:00}초";
        return formattedTime;
    }

    public void SetClearTime(float time,float bestTime)
    {
        clearTimeText.text = TimeFormatting(time);
        bestClearTimeText.text = TimeFormatting(bestTime);
    }
}
