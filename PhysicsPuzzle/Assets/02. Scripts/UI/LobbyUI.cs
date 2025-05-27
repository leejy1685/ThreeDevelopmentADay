using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : BaseUI
{
    [Header("[Lobby UI]")]
    private TextMeshProUGUI title;
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;
    
    protected override UIState GetUIState()
    {
        return UIState.Lobby;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        
        startButton.onClick.AddListener(OnClickStartButton);
        loadButton.onClick.AddListener(OnClickLoadButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }
    
    // 인스펙터에서 직접 연결. 씬에 슬라이더 추가했다고 가정, 슬라이더 설정은 되어 있을거고. 
    // onvalueChangedEvent로. dropDown을 펼쳐서 +버튼을 하신 다음에. 사운드매니저를, 할당해서 우측 함수 목록해서
    // 마스터슬라이드, 셋마스터 넣고, bgm은 bgm넣고. 

    private void Start()
    {
        SoundManager.Instance.Play(SceneManager.GetActiveScene().name);
    }

    private void OnClickStartButton()
    {
        GameManager.Instance.GameStart();
        //UIManager.Instance.OnClickStart();
    }
    
    private void OnClickLoadButton()
    {
  
    }
    
    private void OnClickOptionButton()
    {
        UIManager.Instance.OnClickOption();
    }
    
    private void OnClickExitButton()
    {
        UIManager.Instance.OnClickExit();
    }
}
