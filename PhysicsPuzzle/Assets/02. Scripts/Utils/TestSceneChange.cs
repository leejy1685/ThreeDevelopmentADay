using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneChange : MonoBehaviour
{
    [SerializeField] Button button;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        // 매개변수로 씬타입 값으로 지정
        SceneHandleManager.Instance.LoadScene(SCENE_TYPE.TestGoalScene);
    }
}
