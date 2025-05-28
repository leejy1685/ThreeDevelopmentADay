using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUI : BaseUI
{
    protected override UIState GetUIState()
    {
        return UIState.Clear;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
}
