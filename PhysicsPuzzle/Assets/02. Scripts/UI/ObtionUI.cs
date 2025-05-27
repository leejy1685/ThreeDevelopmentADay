using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtionUI : BaseUI
{
    [Header("[Option UI]")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button backButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        
        backButton.onClick.AddListener(OnClickBackButton);
    }

    private void Update()
    {
        bgmSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetBGMVolume(value));;
        sfxSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetSFXVolume(value));;
    }

    protected override UIState GetUIState()
    {
        return UIState.Obtion;
    }

    public void OnClickBackButton()
    {
        uiManager.ChangeState(UIState.Lobby);
    }
}
