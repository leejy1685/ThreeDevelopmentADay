using System.Collections;
using _02._Scripts.UI;
using _02._Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.Managers.Indestructable
{ 
    public enum UIState

    {
        Lobby,
        Game,
        Clear,
        Obtion,
        LoadingScene
    }
    
    public class UIManager : Singleton<UIManager>
    {
        [Header("[UI State]")]
        [SerializeField] private UIState prevState;
        [SerializeField] public UIState currentState = UIState.Lobby;
        
        [Header("Fade Image")]
        [SerializeField] private Image fadeImage;
        
        // Fields
        private LobbyUI _lobbyUI;
        private OptionUI _optionUI;
        private GameUI _gameUI;
        private ClearUI _clearUI;
        
        // Properties
        public GameUI GameUI => _gameUI;
        public UIState PrevState { get => prevState; }
        
        protected override void Awake()
        {
            base.Awake();
    
            // 자식 오브젝트에서 각각의 UI를 찾아 초기화
            _lobbyUI = GetComponentInChildren<LobbyUI>(true);
            _lobbyUI?.Init(this);
            _optionUI = GetComponentInChildren<OptionUI>(true);
            _optionUI?.Init(this);
            _gameUI = GetComponentInChildren<GameUI>(true);
            _gameUI?.Init(this);
            _clearUI = GetComponentInChildren<ClearUI>(true);
            _clearUI?.Init(this);
    
            // 초기 상태를 로비 화면으로 설정
            ChangeState(UIState.Lobby);
        }
    
        public void ChangeState(UIState state)
        {
            currentState = state;
            
            _lobbyUI?.SetActive(currentState);
            _optionUI?.SetActive(currentState);
            _gameUI?.SetActive(currentState);
            _clearUI?.SetActive(currentState);
        }
        
        public void OnClickExit()
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    
        public void SetClearUI(float time,float bestTime)
        {
            _clearUI.SetClearTime(time,bestTime);
            ChangeState(UIState.Clear);
        }

        public void SetOptionUI()
        {
            if (currentState == UIState.Obtion) { ChangeState(prevState); return; }
            prevState = currentState;
            ChangeState(UIState.Obtion);
        }

        public void SetGameUI(bool timer)
        {
            StartCoroutine(FadeIn_Coroutine());
            GameUI.ChangeSceneName();
            _gameUI.SetTimer(timer);
            ChangeState(UIState.Game);
        }
        
        private IEnumerator FadeIn_Coroutine()
        {
            float elapsedTime = 0f;
            ;
            float fadeDuration = 1f;
            Color color = fadeImage.color;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alphaValue = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alphaValue);
                yield return null;
            }

            fadeImage.color = new Color(color.r, color.g, color.b, 0f);
        }
    }
}


