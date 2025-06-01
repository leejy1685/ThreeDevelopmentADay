using System.Collections;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.UI;
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
        private UIState prevState;
        public UIState currentState = UIState.Lobby;
        LobbyUI lobbyUI = null;
        ObtionUI obtionUI = null;
        GameUI gameUI = null;
        ClearUI clearUI = null;
        
        public GameUI GameUI => gameUI;
        public UIState PrevState { get => prevState; }
        [SerializeField] private Image fadeImage;
    
        protected override void Awake()
        {
            base.Awake();
    
            // 자식 오브젝트에서 각각의 UI를 찾아 초기화
            lobbyUI = GetComponentInChildren<LobbyUI>(true);
            lobbyUI?.Init(this);
            obtionUI = GetComponentInChildren<ObtionUI>(true);
            obtionUI?.Init(this);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameUI?.Init(this);
            clearUI = GetComponentInChildren<ClearUI>(true);
            clearUI?.Init(this);
    
            // 초기 상태를 로비 화면으로 설정
            ChangeState(UIState.Lobby);
        }
    
    
        public void ChangeState(UIState state)
        {
            currentState = state;
            
            lobbyUI?.SetActive(currentState);
            obtionUI?.SetActive(currentState);
            gameUI?.SetActive(currentState);
            clearUI?.SetActive(currentState);
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
            clearUI.SetClearTime(time,bestTime);
            ChangeState(UIState.Clear);
        }

        public void SetOptionUI()
        {
            prevState = currentState;
            ChangeState(UIState.Obtion);
        }

        public void SetGameUI(bool timer)
        {
            StartCoroutine(FadeIn_Coroutine());
            GameUI.ChangeSceneName();
            gameUI.SetTimer(timer);
            ChangeState(UIState.Game);
        }
        
        IEnumerator FadeIn_Coroutine()
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


