using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.GoogleVr;
using UnityEngine;

public class GameUI : BaseUI
{
   
    [Header("Top UI")]
    [SerializeField] private GameObject _nowTImeIcon;
    [SerializeField] private TextMeshProUGUI _stageNameText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [Header("Bottom UI")]
    [SerializeField] private GameObject _changeGravityUI;
    [SerializeField] private GameObject _changeTimeUI;
    [SerializeField] public GameObject InventoryBG;


    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }




}
