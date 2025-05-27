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
        bgmSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    
    private void Start()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        loadButton.onClick.AddListener(OnClickLoadButton);
        optionButton.onClick.AddListener(OnClickOptionButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        
        // bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        
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
