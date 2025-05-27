using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : BaseUI
{
    [Header("[Lobby UI]")]
    private TextMeshProUGUI title;
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;
    
    [Header("[Option UI]")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    
    protected override UIState GetUIState()
    {
        return UIState.Lobby;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    
    // 인스펙터에서 직접 연결. 씬에 슬라이더 추가했다고 가정, 슬라이더 설정은 되어 있을거고. 
    // onvalueChangedEvent로. dropDown을 펼쳐서 +버튼을 하신 다음에. 사운드매니저를, 할당해서 우측 함수 목록해서
    // 마스터슬라이드, 셋마스터 넣고, bgm은 bgm넣고. 
    
    private void Start()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        loadButton.onClick.AddListener(OnClickLoadButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        
        bgmSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetBGMVolume(value));;
        sfxSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetSFXVolume(value));;

        SoundManager.Instance.Play("GravityScene");
    }
    
    private void OnClickStartButton()
    {
        UIManager.Instance.OnClickStart();
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
