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

        LoadData();

        clearTimeText.text = TimeFormatting(UIManager.Instance.CurrentClearTime);
        bestClearTimeText.text = TimeFormatting(bestScore);
        lobbyButton.onClick.AddListener(OnClickLobbyButton);
    }

    private void OnClickLobbyButton()
    {
        UIManager.Instance.OnClickStart();
    }

    // 데이터 불러오기
    private void LoadData()
    {
        
        if (System.Enum.TryParse(SceneManager.GetActiveScene().name, out sceneType))
        {
            bestScore = UIManager.Instance.LoadBestScore(sceneType);
        }
        else
        {
            Debug.LogWarning("현재 씬이 enum에 없습니다.");
        }
        
        // 테스트용 코드. 50 = 클리어, 51 = 클리어time 저장
        // PlayerPrefs.SetInt(sceneType.ToString(),1); 
        // PlayerPrefs.SetFloat($"{sceneType}_BestClearTime", 1500f);;
    }

    // float 값 분:초로 변환
    private string TimeFormatting(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        formattedTime = $"{minutes:00}분 {seconds:00}초";
        return formattedTime;
    }
}
